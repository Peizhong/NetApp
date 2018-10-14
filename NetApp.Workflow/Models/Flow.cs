using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace NetApp.Workflow.Models
{
    public class Flow
    {
        public Flow()
        {
            FlowId = Guid.NewGuid().ToString();
            CreateTime = DateTime.Now;
        }

        public string FlowId { get; private set; }

        public DateTime CreateTime { get; private set; }

        public string FlowName { get; set; }
        
        public FlowConfig FlowConfig { get; set; }

        public IServiceProvider ServiceProvider { get; set; }

        private List<Node> _loadedNodes = new List<Node>();

        public IEnumerable<Node> CurrentNodes => _loadedNodes.Where(n => n.NodeStatus != EnumNodeStatus.End);

        private Node ActivateNode(string typeName)
        {
            Node nextNode = (Node)Activator.CreateInstance(Type.GetType(typeName));
            if (nextNode != null)
            {
                nextNode.Flow = this;
                nextNode.FlowId = this.FlowId;
                nextNode.NodeType = typeName;
                nextNode.StatusTime = DateTime.Now;
                //nextNode.StartMode
                //nextNode.NodeStatus = EnumNodeStatus.Create;
            }
            return nextNode;
        }

        public void MoveOn(string command, string data)
        {
            //第一次加载入口节点
            if (_loadedNodes.Count < 1)
            {
                var nextNode = ActivateNode(FlowConfig.EntranceNodeNodeType);
                nextNode.StartMode = EnumNodeStartMode.Direct;
                nextNode.NodeStatus = EnumNodeStatus.Create;
                _loadedNodes.Add(nextNode);
            }
            Parallel.ForEach(CurrentNodes, async node =>
            {
                //执行完毕后，定位下一次节点
                if (EnumNodeStatus.Complete == await node.TryExecute(command, data))
                {
                    var currentNode = FlowConfig.AvailableNodes.Single(n => n.NodeType == node.NodeType);
                    //Todo: 要执行哪个?
                    foreach (var nx in currentNode.NextNodes)
                    {
                        //生成下一个执行节点
                        Node nextNode = ActivateNode(nx.Value.NodeType);
                        nextNode.StartMode = nx.Value.StartMode;
                        nextNode.NodeStatus = EnumNodeStatus.Create;
                        nextNode.PreviousNode.Add(node);
                        _loadedNodes.Add(nextNode);
                    }
                }
            });
            //执行新创建的节点
            var newNode = _loadedNodes.Where(n => n.NodeStatus == EnumNodeStatus.Create);
            while (newNode.Any())
            {
                Parallel.ForEach(newNode, async node =>
                {                    
                    //执行完毕后，定位下一次节点
                    var previousNode = node.PreviousNode.Where(p => p.NodeStatus == EnumNodeStatus.Complete).OrderByDescending(p => p.StatusTime).FirstOrDefault();
                    if (EnumNodeStatus.Complete == await node.TryExecute(previousNode?.OutputCommand, previousNode?.OutputData))
                    {
                        var nextNodes = FlowConfig.AvailableNodes.Where(n => n.NodeType == node.NodeType);
                        foreach (var nx in nextNodes)
                        {
                            //生成下一个执行节点
                            Node nextNode = ActivateNode(nx.NodeType);
                            nextNode.StartMode = nx.StartMode;
                            nextNode.NodeStatus = EnumNodeStatus.Create;
                            nextNode.PreviousNode.Add(node);
                            _loadedNodes.Add(nextNode);
                        }
                    }
                });
                newNode = _loadedNodes.Where(n => n.NodeStatus == EnumNodeStatus.Create);
            }
        }
    }
}
