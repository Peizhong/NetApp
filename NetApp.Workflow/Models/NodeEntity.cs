using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NetApp.Workflow.Models
{
    /// <summary>
    /// 工作流在数据库表示，不能执行
    /// </summary>
    public class NodeEntity
    {
        [Key]
        public string Id { get; set; }

        public string NodeId { get; set; }

        public string PreviousNodeId { get; set; }

        /// <summary>
        /// 前面节点传过来的命令
        /// </summary>
        public string ReceivedCommand { get; set; }

        /// <summary>
        /// 前面节点传过来的数据
        /// </summary>
        public string ReceivedData { get; set; }

        public string FlowId { get; set; }

        public string NodeType { get; set; }

        public EnumNodeStatus NodeStatus { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime StatusDate { get; set; }
    }
}
