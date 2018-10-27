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
using Microsoft.Extensions.Logging;

namespace NetApp.Workflow
{
    public class WorkflowFactory
    {
        private readonly ILogger<WorkflowFactory> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentDictionary<string, FlowConfig> _workflowConfigDict;

        public WorkflowFactory(ILogger<WorkflowFactory> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;

            using (var db = new WorkflowDbContext())
            {
                //db.Database.EnsureDeleted();

                //db.Database.EnsureCreated();
            }
            _serviceProvider = serviceProvider;
            _workflowConfigDict = new ConcurrentDictionary<string, FlowConfig>();
        }

        private FlowConfig LoadFlowConfigFromJson(string data)
        {
            JObject raw = JsonConvert.DeserializeObject<JObject>(data);
            FlowConfig flowConfig = new FlowConfig
            {
                ConfigJson = data,
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

        private FlowConfig LoadFlowConfigFromFile(string path)
        {
            var d = Directory.GetCurrentDirectory();
            if (!File.Exists(path))
                return null;
            string json = File.ReadAllText(path);
            var config = LoadFlowConfigFromJson(json);
            config.ConfigName = path;
            return config;
        }

        private void SaveFlowToDb(Flow flow)
        {
            using (var db = new WorkflowDbContext())
            {
                FlowEntity flowEntity = new FlowEntity
                {
                    FlowId = flow.FlowId,
                    ConfigName = flow.FlowConfig.ConfigName,
                    ConfigJson = flow.FlowConfig.ConfigJson,
                    CreateTime = flow.CreateTime,
                };
                db.FlowEntities.Add(flowEntity);
                db.SaveChanges();
            }
        }

        /// <summary>
        /// 创建工作流，并设置第一个节点
        /// </summary>
        /// <param name="flowName"></param>
        /// <param name="configName"></param>
        /// <returns></returns>
        public Flow CreateWorkflow(string flowName, string configName)
        {
            var config = _workflowConfigDict.GetOrAdd(configName, name => LoadFlowConfigFromFile(name));
            var flow = new Flow
            {
                FlowId = Guid.NewGuid().ToString(),
                CreateTime = DateTime.Now,
                FlowConfig = config,
                ServiceProvider = _serviceProvider,
                Nodes = new List<NodeEntity>(),
            };
            var entranceNode = new NodeEntity
            {
                Id = Guid.NewGuid().ToString(),
                NodeId = Guid.NewGuid().ToString(),
                FlowId = flow.FlowId,
                NodeType = flow.FlowConfig.EntranceNodeNodeType,
                NodeStatus = EnumNodeStatus.Create,
                CreateTime = DateTime.Now,
                StatusTime = DateTime.Now,
                //PreviousNodeId = "",
                //ReceivedCommand = "",
                //ReceivedData = ""
            };
            flow.Nodes.Add(entranceNode);
            SaveFlowToDb(flow);
            return flow;
        }

        public async Task<Flow> FindWorkflow(string flowId)
        {
            using (var db = new WorkflowDbContext())
            {
                var flowsInDb = await db.FlowEntities.AsNoTracking().Where(f => f.FlowId == flowId).SingleOrDefaultAsync();
                if (flowsInDb == null)
                    return null;
                FlowConfig flowConfig = null;
                if (!string.IsNullOrEmpty(flowsInDb.ConfigJson))
                {
                    flowConfig = LoadFlowConfigFromJson(flowsInDb.ConfigJson);
                }
                else
                {
                    flowConfig = _workflowConfigDict.GetOrAdd(flowsInDb.ConfigName, name => LoadFlowConfigFromFile(name));
                }
                var flow = new Flow
                {
                    FlowId = flowsInDb.FlowId,
                    CreateTime = flowsInDb.CreateTime,
                    FlowConfig = flowConfig,
                    Nodes = db.NodeEntities.Where(n => n.FlowId == flowId).ToList(),
                    ServiceProvider = _serviceProvider,
                };
                return flow;
            }
        }
    }
}
