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
        private static object locker = new object();
        private static WorkflowFactory _instance = null;

        public static WorkflowFactory Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (locker)
                    {
                        if (_instance == null)
                            _instance = new WorkflowFactory();
                    }
                }
                return _instance;
            }
        }

        private ConcurrentDictionary<string, JObject> configDict;
        private ConcurrentDictionary<string, Flow> workflowDict;

        private WorkflowFactory()
        {
            using (var db = new WorkflowDbContext())
            {
                db.Database.EnsureDeleted();

                db.Database.EnsureCreated();
            }
            configDict = new ConcurrentDictionary<string, JObject>();
            workflowDict = new ConcurrentDictionary<string, Flow>();
        }

        public Flow CreateWorkflow(string flowName, string configName)
        {
            if (!configDict.TryGetValue(configName, out JObject obj))
            {
                var d = Directory.GetCurrentDirectory();
                if (File.Exists(configName))
                {
                    string json = File.ReadAllText(configName);
                    obj = JsonConvert.DeserializeObject<JObject>(json);
                    configDict.TryAdd(configName, obj);
                    //JArray array = obj["AvailableNodes"] as JArray;
                }
            }
            var entranceNode = obj["EntranceNode"].ToString();
            //Type t = Type.GetType(entranceNode);
            var flow = new Flow
            {
                FlowName = obj["FlowName"].ToString(),
                EntranceNodeType = entranceNode,
            };
            workflowDict.TryAdd(flow.FlowId, flow);
            return flow;
        }

        public Flow FindFlow(string flowId)
        {
            if (workflowDict.TryGetValue(flowId, out Flow flow))
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
