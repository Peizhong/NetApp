using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NetApp.Models
{
    public class WorkflowRef
    {
        [Key]
        public string Id { get; set; }

        public string WorkflowId { get; set; }

        public string ObjectId { get; set; }
    }
}
