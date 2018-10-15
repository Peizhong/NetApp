using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace NetApp.Workflow.Models
{
    public class FlowEntity
    {
        [Key]
        public string FlowId { get; set; }

        public string ConfigName { get; set; }

        public string ConfigJson { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
