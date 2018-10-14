using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;
using NetApp.Workflow.Models;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace NetApp.Workflow
{
    public class WorkflowFactory
    {
        private static object _locker = new object();
        private static WorkflowFactory _instance = null;

        public static WorkflowFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_locker)
                    {
                        if (_instance == null)
                            _instance = new WorkflowFactory();
                    }
                }
                return _instance;
            }
        }

        private ConcurrentDictionary<string, Flow> _workflowDict;

        private WorkflowFactory()
        {
            using (var db = new WorkflowDbContext())
            {
                db.Database.EnsureDeleted();

                db.Database.EnsureCreated();
            }
            _workflowDict = new ConcurrentDictionary<string, Flow>();
        }

        public FlowConfig LoadConfig(string path)
        {
            var d = Directory.GetCurrentDirectory();
            if (!File.Exists(path))
                return null;
            string json = File.ReadAllText(path);
            JObject raw = JsonConvert.DeserializeObject<JObject>(json);
            FlowConfig flowConfig = new FlowConfig
            {
                FlowName = raw["FlowName"].ToString(),
                EntranceNodeNodeType = raw["EntranceNode"].ToString(),
                AvailableNodes = new List<NodeConfig>()
            };
            JArray availableNodes = raw["AvailableNodes"] as JArray;
            foreach (var row in availableNodes)
            {
                EnumNodeStartMode enumStartMode = EnumNodeStartMode.Any;
                var startMode = row["StartMode"].ToString();
                if (!string.IsNullOrEmpty(startMode))
                {
                    enumStartMode = (EnumNodeStartMode)Enum.Parse(typeof(EnumNodeStartMode), startMode);
                }
                var nc = new NodeConfig
                {
                    NodeType = row["NodeType"].ToString(),
                    NodeDescription = row["NodeDescription"].ToString(),
                    StartMode = enumStartMode,
                    NextNodes = new Dictionary<string, NodeConfig>(),
                };
                flowConfig.AvailableNodes.Add(nc);
            }
            foreach (var row in availableNodes)
            {
                var nodeType = row["NodeType"].ToString();
                var currentNode = flowConfig.AvailableNodes.FirstOrDefault(c => c.NodeType == nodeType);
                if (currentNode == null)
                    continue;
                JArray nextNodes = row["NextNodes"] as JArray;
                if (nextNodes == null)
                    continue;
                //JObject->JContainer->JToken
                foreach (JObject nn in nextNodes)
                {
                    using (var enumerator = nn.GetEnumerator())
                    {
                        if (enumerator.MoveNext())
                        {
                            var item = enumerator.Current;
                            var key = item.Key;
                            var nextType = item.Value.ToString();
                            var nextTarget = flowConfig.AvailableNodes.FirstOrDefault(c => c.NodeType == nextType);
                            if (nextTarget != null)
                            {
                                currentNode.NextNodes.Add(key, nextTarget);

                            }
                        }
                    }
                }
            }
            return flowConfig;
        }

        public Flow CreateWorkflow(string flowName, string configName, IServiceProvider serviceProvider)
        {
            var flow = new Flow
            {
                FlowName = flowName,
                FlowConfig = LoadConfig(configName),
                ServiceProvider = serviceProvider
            };
            _workflowDict.TryAdd(flow.FlowId, flow);
            return flow;
        }

        public Flow FindFlow(string flowId)
        {
            if (_workflowDict.TryGetValue(flowId, out Flow flow))
                return flow;
            return null;
        }

        /// <summary>
        /// 获取指定类型的节点
        /// </summary>
        /// <returns></returns>
        public async Task<List<NodeEntity>> GetTypeNode(string path)
        {
            using (var db = new WorkflowDbContext())
            {
                var node = await db.NodeEntities.AsNoTracking().Where(n => n.NodeType.StartsWith(path)).ToListAsync();
                return node;
            }
        }

        /// <summary>
        /// 尾部添加
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public bool Append(Node node)
        {
            return true;
        }

        /// <summary>
        /// 头部插入
        /// </summary>
        /// <param name="flow"></param>
        /// <returns></returns>
        public bool Insert(Flow flow)
        {
            return true;
        }

        public void MoveOn(string flowId, string command, string data)
        {
            var flow = FindFlow(flowId);
            flow?.MoveOn(command, data);
        }
    }
}
