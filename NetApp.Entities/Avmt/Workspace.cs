using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetApp.Entities.Avmt
{
    [Table("dm_workspace")]
    public class Workspace
    {
        [Key]
        public string Id { get; set; }

        [Column("WORKSPACE_ID")]
        public string WorkspaceName { get; set; }

        [Column("BUSINESS_BILL_ID")]
        public string BusinessBillId { get; set; }

        [Column("OBJECT_ID")]
        public string RootId { get; set; }

        [Column("UPDATE_TIME")]
        public DateTime UpdateTime { get; set; }
        
    }
}
