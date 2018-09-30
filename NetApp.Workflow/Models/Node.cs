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
        End = Complete | Abort
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
        public string NodeId { get; set; }

        public abstract string NodeTypeName { get; }

        public EnumNodeStatus NodeStatus { get; set; }

        /// <summary>
        /// 执行的条件
        /// </summary>
        public EnumNodeExcuteCondition ExcuteCondition { get; set; }
        
        public List<Node> PreviousNode { get; set; }

        public virtual bool IsCanExcute(string command)
        {
            if (PreviousNode != null)
            {
                if (ExcuteCondition == EnumNodeExcuteCondition.Direct)
                    return true;
                if (ExcuteCondition == EnumNodeExcuteCondition.All)
                    return PreviousNode.All(n => n.NodeStatus == EnumNodeStatus.Complete);
                if (ExcuteCondition == EnumNodeExcuteCondition.Any)
                    return PreviousNode.Any(n => n.NodeStatus == EnumNodeStatus.Complete);
            }
            return false;
        }

        public virtual string DoWork()
        {
            return "ok";
        }

        public string TryExcute(string command, string data)
        {
            var result = string.Empty;
            if (IsCanExcute(command))
                return DoWork();
            return result;
        }
    }
}
