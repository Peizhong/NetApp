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
        [Key]
        public string NodeId { get; set; }

        public string FlowId { get; set; }

        public string NodeType { get; set; }

        /// <summary>
        /// NodeType-NextNodeType 多对多
        /// </summary>
        public string NextNodeType { get; set; }

        public EnumFlowStatus FlowStatus { get; set; }
        
        public virtual ICollection<NodeEntity> PreviousFlows { get; set; }

        public static implicit operator NodeEntity(Flow flow)
        {
            return new NodeEntity
            {
                NodeId = flow.FlowId
            };
        }
    }
}
