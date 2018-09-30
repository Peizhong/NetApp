using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace NetApp.Workflow.Models
{
    public enum EnumNodeStatus
    {
        None = 0,
        Create = 0x1,
        Suspend = 0x10,
        Complete = 0x100,
        Abort = 0x1000,
        Error = 0x10000,
        End = Complete | Abort | Error
    }

    public enum EnumNodeExcuteCondition
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

    public abstract class Node
    {
        public string NodeId { get; private set; }

        public Flow Flow { get; set; }

        public abstract string NodeName { get; }

        public abstract string NodeDescription { get; protected set; }

        /// <summary>
        /// assemly
        /// </summary>
        public abstract string NodeType { get; protected set; }

        public DateTime CreateTime { get; private set; }

        /// <summary>
        /// 当前状态时间
        /// </summary>
        public DateTime StatusTime { get; protected set; }

        /// <summary>
        /// 节点结束时间
        /// </summary>
        public DateTime? EndTime { get; protected set; }

        /// <summary>
        /// 执行的条件
        /// </summary>
        public EnumNodeExcuteCondition ExcuteCondition { get; protected set; }
        
        public EnumNodeStatus NodeStatus { get; protected set; }

        public List<Node> PreviousNode { get; set; }

        protected string _command;
        protected string _data;

        public Node()
        {
            NodeId = Guid.NewGuid().ToString();
            CreateTime = DateTime.Now;
            StatusTime = CreateTime;
        }

        /// <summary>
        /// do noting, need to be overrided 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public virtual bool IsCanExcute(string command)
        {
            return true;
        }

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
            if (IsCanExcute(command))
            {
                _command = command;
                _data = data;
                await DoWork();
            }
            return NodeStatus;
        }
    }
}
