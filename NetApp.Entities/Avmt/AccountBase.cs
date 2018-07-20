using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetApp.Entities.Avmt
{
    public class AccountBase
    {
        [Column("id")]
        public string Id { get; set; }

        [Column("workspace_id")]
        public string WorkspaceId { get; set; }

        [Column("province_code")]
        public string ProvinceCode { get; set; }

        [Column("bureau_code")]
        public string BureauCode { get; set; }

        [Column("classify_id")]
        public string ClassifyId { get; set; }

        [Column("base_voltage_id")]
        public int VoltageId { get; set; }

        [Column("update_time")]
        public DateTime UpdateTime { get; set; }

        [Column("remark")]
        public string Remark { get; set; }
    }
}