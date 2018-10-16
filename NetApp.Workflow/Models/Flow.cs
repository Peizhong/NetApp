using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace NetApp.Workflow.Models
{
    public class Flow
    {
        public string FlowId { get; set; }

        public FlowConfig FlowConfig { get; set; }

        public DateTime CreateTime { get; set; }

        public IServiceProvider ServiceProvider { get; set; }
        
        public List<NodeEntity> Nodes { get; set; }

        private Node ActivateNode(string typeName)
        {
            try
            {
                Node nextNode = (Node)Activator.CreateInstance(Type.GetType(typeName));
                nextNode.Flow = this;
                nextNode.NodeType = typeName;
                //nextNode.NodeStatus = EnumNodeStatus.Create;
                return nextNode;
            }
            catch
            {
                return null;
            }
        }
        
        /// <summary>
        /// 执行当前节点，如果完成了，生成下个节点继续执行
        /// </summary>
        /// <param name="nodeType"></param>
        /// <param name="command"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private async Task RecursiveMoveOn(string nodeType, string command, string data)
        {
            var targetNode = Nodes.SingleOrDefault(n => n.NodeType == nodeType && n.NodeStatus != EnumNodeStatus.Complete);
            if (targetNode == null)
                return;
            var workingNode = ActivateNode(nodeType);
            if (workingNode == null)
                return;
            await workingNode.TryExecute(command, data);
            //执行后，更新entity
            targetNode.NodeStatus = workingNode.NodeStatus;
            targetNode.StatusTime = workingNode.StatusTime;
            targetNode.OutputCommand = workingNode.OutputCommand;
            targetNode.OutputData = workingNode.OutputData;
            if (workingNode.NodeStatus == EnumNodeStatus.Complete)
            {
                //FlowConfig最开始的数据是根据配置文件来的，
                var currentNodeConfig = FlowConfig.AvailableNodes.SingleOrDefault(n => n.NodeType == nodeType);
                if (currentNodeConfig == null)
                    return;
                //下级节点全部要执行
                foreach (var nx in currentNodeConfig.NextNodes.Values)
                {
                    var nextNode = new NodeEntity
                    {
                        Id = Guid.NewGuid().ToString(),
                        NodeId = Guid.NewGuid().ToString(),
                        FlowId = FlowId,
                        NodeType = nx.NodeType,
                        NodeStatus = EnumNodeStatus.Create,
                        CreateTime = DateTime.Now,
                        StatusTime = DateTime.Now,
                        PreviousNodeId = targetNode.NodeId,
                        InputCommand = targetNode.OutputCommand,
                        InputData = targetNode.OutputData
                    };
                    Nodes.Add(nextNode);
                    await RecursiveMoveOn(nx.NodeType, targetNode.OutputCommand, targetNode.OutputData);
                }
            }
        }

        private async Task UpdateNodeEntityDb()
        {
            using (var context = new WorkflowDbContext())
            {
                foreach (var node in Nodes)
                {
                    var dbNode = context.NodeEntities.Find(node.Id);
                    if (dbNode == null)
                    {
                        context.NodeEntities.Add(node);
                    }
                    else
                    {
                        context.Entry(dbNode).CurrentValues.SetValues(node);
                    }
                }
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// 每个工作流，每种类型的节点只有一个是正在运行的
        /// </summary>
        /// <param name="nodeType"></param>
        /// <param name="data"></param>
        public async Task MoveOn(string nodeType, string command, string data)
        {
            //每次moveon，先写入内存，最后保存数据库
            await RecursiveMoveOn(nodeType, command, data);
            await UpdateNodeEntityDb();
        }
    }
}
