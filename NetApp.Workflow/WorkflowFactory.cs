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
        private readonly ConcurrentDictionary<string, Flow> _workflowDict;

        public WorkflowFactory(ILogger<WorkflowFactory> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;

            using (var db = new WorkflowDbContext())
            {
                //db.Database.EnsureDeleted();

               // db.Database.EnsureCreated();
            }
            _serviceProvider = serviceProvider;
            _workflowConfigDict = new ConcurrentDictionary<string, FlowConfig>();
            _workflowDict = new ConcurrentDictionary<string, Flow>();
            LoadFlowFromDb();
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
        /// 从数据库中加载工作流
        /// </summary>
        private void LoadFlowFromDb()
        {
            using (var db = new WorkflowDbContext())
            {
                var flowsInDb = db.FlowEntities.AsNoTracking().ToList();
                foreach (var fl in flowsInDb)
                {
                    FlowConfig flowConfig = null;
                    if (!string.IsNullOrEmpty(fl.ConfigJson))
                    {
                        flowConfig = LoadFlowConfigFromJson(fl.ConfigJson);
                    }
                    else
                    {
                        flowConfig = _workflowConfigDict.GetOrAdd(fl.ConfigName, name => LoadFlowConfigFromFile(name));
                    }
                    var flow = new Flow
                    {
                        FlowId = fl.FlowId,
                        CreateTime = fl.CreateTime,
                        FlowConfig = flowConfig,
                        NodeEntities = db.NodeEntities.AsNoTracking().Where(n => n.FlowId == fl.FlowId).ToList(),
                        ServiceProvider = _serviceProvider,
                    };
                    _workflowDict.TryAdd(flow.FlowId, flow);
                }
            }
        }

        public Flow CreateWorkflow(string flowName, string configName)
        {
            var config = _workflowConfigDict.GetOrAdd(configName, name => LoadFlowConfigFromFile(name));
            var flow = new Flow
            {
                FlowId = Guid.NewGuid().ToString(),
                FlowConfig = config,
                ServiceProvider = _serviceProvider
            };
            _workflowDict.TryAdd(flow.FlowId, flow);
            SaveFlowToDb(flow);
            return flow;
        }

        public Flow FindWorkflow(string flowId)
        {
            _workflowDict.TryGetValue(flowId, out Flow flow);
            return flow;
        }

        public void MoveOn(string flowId, string command, string data)
        {
            if (_workflowDict.TryGetValue(flowId, out Flow flow))
            {
                flow.MoveOn(command, data);
                //TODO: 更新数据库
            }
        }
    }
}
