using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetApp.Workflow.Models
{
    public enum EnumNodeStatus
    {
        None = 0,
        Create = 0x1,
        Excuting = 0x10,
        Complete = 0x100,
        Abort = 0x1000,
        Error = 0x10000,
        End = Complete | Abort | Error
    }

    public enum EnumNodeStartMode
    {
        /// <summary>
        /// 直接往下执行
        /// </summary>
        Direct,
        /// <summary>
        /// 上级全部执行完毕
        /// </summary>
        All,
        /// <summary>
        /// 任意一个上级执行完毕
        /// </summary>
        Any,
    }

    public class Node
    {
        public Node()
        {
            NodeId = Guid.NewGuid().ToString();
            CreateTime = DateTime.Now;
        }

        public string NodeId { get; private set; }

        public string FlowId { get; set; }

        public Flow Flow { get; set; }

        /// <summary>
        /// assemly
        /// </summary>
        public string NodeType { get; set; }

        public DateTime CreateTime { get; private set; }
        
        /// <summary>
        /// 当前状态时间
        /// </summary>
        public DateTime StatusTime { get; set; }

        /// <summary>
        /// 用三个节点进入的条件
        /// </summary>
        public EnumNodeStartMode StartMode { get; set; }
        
        public EnumNodeStatus NodeStatus { get; set; }

        [NotMapped]
        public List<Node> PreviousNode { get; set; } = new List<Node>();

        protected string _inputCommand;
        protected string _inputData;

        public string OutputCommand { get; set; }
        public string OutputData { get; set; }

        /// <summary>
        /// base function do nothing, then completed the node
        /// </summary>
        public virtual Task DoWork()
        {
            NodeStatus = EnumNodeStatus.Complete;
            return Task.CompletedTask;
        }

        public async Task<EnumNodeStatus> TryExecute(string command, string data)
        {
            //新创建的节点，都先标记为执行中
            if (NodeStatus == EnumNodeStatus.Create)
            {
                NodeStatus = EnumNodeStatus.Excuting;
            }
            _inputCommand = command;
            _inputData = data;
            await DoWork();
            return NodeStatus;
        }
    }
}
