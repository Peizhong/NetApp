using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace NetApp.Workflow.Models
{
    public class Flow
    {
        public string FlowId { get; set; }
       
        public FlowConfig FlowConfig { get; set; }
        
        public DateTime CreateTime { get; set; }

        public IServiceProvider ServiceProvider { get; set; }

        public List<NodeEntity> NodeEntities { get; set; }

        private List<Node> _loadedNodes = new List<Node>();

        public IEnumerable<Node> UnfinishedNode => _loadedNodes.Where(n => n.NodeStatus != EnumNodeStatus.End);

        /// <summary>
        /// 从配置中查找当前节点类型可能的上级节点
        /// </summary>
        /// <param name="nodeType"></param>
        /// <returns></returns>
        public IEnumerable<NodeConfig> GetPreviousNodeConfig(string nodeType)
        {
            return FlowConfig.AvailableNodes.SelectMany(n => n.NextNodes).Where(n => n.Key == nodeType).Select(k => k.Value);
        }

        /// <summary>
        /// 查找已创建的节点
        /// </summary>
        /// <param name="nodeType"></param>
        /// <returns></returns>
        public string GetExistedNodeId(string nodeType)
        {
            using (var context = ServiceProvider.GetRequiredService<WorkflowDbContext>())
            {
                var node = context.NodeEntities.SingleOrDefault(n => n.FlowId == FlowId && n.NodeType == nodeType && n.NodeStatus != EnumNodeStatus.Complete);
                return node?.NodeId;
            }
        }

        private Node ActivateNode(string typeName)
        {
            try
            {
                Node nextNode = (Node)Activator.CreateInstance(Type.GetType(typeName));

                nextNode.Flow = this;
                nextNode.FlowId = this.FlowId;
                nextNode.NodeType = typeName;
                nextNode.StatusTime = DateTime.Now;
                //nextNode.StartMode
                //nextNode.NodeStatus = EnumNodeStatus.Create;
                return nextNode;
            }
            catch
            {
                return null;
            }
        }

        public void MoveOn(string command, string data)
        {
            //第一次加载入口节点
            if (NodeEntities.Count < 1)
            {
                var entranceNode = ActivateNode(FlowConfig.EntranceNodeNodeType);
                if (entranceNode != null)
                {
                    entranceNode.StartMode = EnumNodeStartMode.Direct;
                    entranceNode.NodeStatus = EnumNodeStatus.Create;
                    _loadedNodes.Add(entranceNode);
                    NodeEntities.Add(new NodeEntity
                    {
                        Id = Guid.NewGuid().ToString(),
                        NodeId = entranceNode.NodeId,
                        FlowId = FlowId,
                        NodeType = entranceNode.NodeType,
                        NodeStatus = EnumNodeStatus.Create,
                        CreateDate = DateTime.Now,
                        StatusDate = DateTime.Now,
                        //PreviousNodeId = "",
                        //ReceivedCommand = "",
                        //ReceivedData = ""
                    });
                }
            }
            Parallel.ForEach(UnfinishedNode, async node =>
            {
                //执行完毕后，定位下一次节点
                if (EnumNodeStatus.Complete == await node.TryExecute(command, data))
                {
                    //TODO: 是哪里决定下个节点？ 节点内部或工作流?
                    //还有一种是动态创建工作流，在节点内部自己创建
                    //FlowConfig最开始的数据是根据配置文件来的，
                    var currentNodeConfig = FlowConfig.AvailableNodes.Single(n => n.NodeType == node.NodeType);
                    //这个是根据配置文件生成下个节点
                    foreach (var nx in currentNodeConfig.NextNodes)
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
