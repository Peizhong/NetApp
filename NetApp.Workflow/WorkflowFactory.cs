using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;
using NetApp.Workflow.Models;

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

        private ConcurrentDictionary<string, Flow> workflowDict;

        private WorkflowFactory()
        {
            using (var db = new WorkflowDbContext())
            {
                db.Database.EnsureDeleted();

                db.Database.EnsureCreated();
            }

            workflowDict = new ConcurrentDictionary<string, Flow>();
        }

        public Flow CreateWorkflow(string flowName, string configName)
        {
            Type t = Type.GetType("NetApp.Workflow.CreateOrderNode, NetApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
            var node =(Node)Activator.CreateInstance(t);

            var flow = new Flow(flowName, configName);
            workflowDict.TryAdd(flow.FlowId, flow);
            return flow;
        }

        public Flow FindFlow(string flowId)
        {
            if (workflowDict.TryGetValue(flowId, out Flow flow))
                return flow;
            return null;
        }

        public bool Append(Flow flow)
        {
            return true;
        }

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
