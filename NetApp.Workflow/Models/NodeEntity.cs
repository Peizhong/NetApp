using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NetApp.Workflow.Models
{
    /// <summary>
    /// node与node之间的关系，可能是多对多
    /// </summary>
    public class NodeRelationEntity
    {
        [Key]
        public string Id { get; set; }

        public string StartNodeId { get; set; }

        public string EndNodeId { get; set; }
    }

    /// <summary>
    /// 工作流在数据库表示，不能执行
    /// </summary>
    public class NodeEntity
    {
        public NodeEntity()
        {
            NodeId = Guid.NewGuid().ToString();
        }

        [Key]
        public string NodeId { get; private set; }

        public string FlowId { get; set; }

        public string NodeType { get; set; }

        public EnumNodeStatus NodeStatus { get; set; }

        /// <summary>
        /// 前面的节点完成了，下面的才能继续
        /// </summary>
        public virtual ICollection<NodeEntity> PreviousFlows { get; set; }

    }
}
