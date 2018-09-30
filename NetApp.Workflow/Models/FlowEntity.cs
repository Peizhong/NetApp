using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NetApp.Workflow.Models
{
    /// <summary>
    /// 工作流在数据库表示
    /// </summary>
    public class FlowEntity
    {
        [Key]
        public string FlowId { get; set; }

        public string FlowName { get; set; }

        public EnumFlowStatus FlowStatus { get; set; }

        public static implicit operator FlowEntity(Flow flow)
        {
            return new FlowEntity
            {
                FlowId = flow.FlowId
            };
        }
    }
}
