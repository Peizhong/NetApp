using System;
using System.Collections.Generic;
using System.Text;

namespace NetApp.Workflow.Models
{
    public enum EnumFlowStatus
    {
        None = 0,
        Create = 0x1,
        Suspend = 0x10,
        Complete = 0x100,
        Abort = 0x1000,
        End = Complete | Abort
    }

    public class Flow
    {
        public Flow(string flowName, string configName)
        {
            FlowId = Guid.NewGuid().ToString();
            FlowName = flowName;
        }

        public string FlowId { get; private set; }

        public string FlowName { get; private set; }

        public EnumFlowStatus FlowStatus { get; set; }

        private Node _entraceNode;

        private Dictionary<string, Node> _loadedNodes = new Dictionary<string, Node>();

        private List<Node> _currentNode = new List<Node>();

        public void MoveOn(string command, string data)
        {
            List<Node> nextNodes = new List<Node>();
            foreach(var node in _currentNode)
            {
                node.TryExcute(command, data);
                //执行完毕后，定位下一次节点
                if (node.NodeStatus == EnumNodeStatus.Complete)
                {
                    //根据配置读取节点
                   // nd.PreviousNode.Add(node);
                }
            }
            _currentNode = nextNodes;
        }
    }
}
