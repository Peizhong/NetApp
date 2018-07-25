using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetApp.Entities.Avmt
{
    public abstract class BillBase
    {
        [Key]
        public string Id { get; set; }

        [Column("province_code")]
        public string ProvinceCode { get; set; }

        [Column("bureau_code")]
        public string BureauCode { get; set; }

        [Column("update_time")]
        public DateTime UpdateTime { get; set; }

        [NotMapped]
        public List<Workspace> Workspaces { get; set; }
    }

    [Table("dm_main_transfer")]
    public class MainTransferBill : BillBase
    {
        [Column("BUSINESS_BILL_CODE")]
        public string BusinessBillCode { get; set; }

        [Column("BUSINESS_BILL_NAME")]
        public string BusinessBillName { get; set; }

        [Column("TRANSFER_STATE")]
        public int TransferState { get; set; }
    }

    [Table("dm_dis_transfer")]
    public class DisTransferBill : BillBase
    {
        [Column("business_code")]
        public string BusinessCode { get; set; }

        [Column("business_name")]
        public string BusinessName { get; set; }

        [Column("business_content")]
        public string BusinessContent { get; set; }
    }

    [Table("dm_change_bill")]
    public class ChangeBill : BillBase
    {
        [Column("CHANGE_CODE")]
        public string ChangeCode { get; set; }

        [Column("BUSINESS_CONTENT")]
        public string BusinessContent { get; set; }

        [Column("POWER_GRID_FLAG")]
        public int PowerGridFlag { get; set; }
    }
}