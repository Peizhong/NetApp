using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
        public Flow()
        {
            FlowId = Guid.NewGuid().ToString();
        }

        public string FlowId { get; private set; }

        public string FlowName { get; set; }
        
        public EnumFlowStatus FlowStatus { get; set; }

        public string EntranceNodeType { get; set; }

        private Node _entraceNode;

        private Dictionary<string, Node> _loadedNodes = new Dictionary<string, Node>();

        private List<Node> _currentNode = new List<Node>();

        public void MoveOn(string command, string data)
        {
            List<Node> nextNodes = new List<Node>();
            Parallel.ForEach(_currentNode, async node =>
            {
                //执行完毕后，定位下一次节点
                if (EnumNodeStatus.Complete == await node.TryExecute(command, data))
                {
                    //根据配置读取节点
                    // nd.PreviousNode.Add(node);
                }
            });
            _currentNode = nextNodes;
        }
    }
}
