using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetApp.Entities.Avmt
{
    [Table("DM_CLASSIFY")]
    public class Classify
    {
        [Key]
        public string Id { get; set; }

        [Column("PARENT_CLASSIFY_ID")]
        public string ParentClassifyId { get; set; }

        [Column("CLASSIFY_NAME")]
        public string ClassifyName { get; set; }

        [Column("FULL_NAME")]
        public string FullName { get; set; }

        [Column("CLASSIFY_TYPE")]
        public int ClassifyType { get; set; }

        [Column("BASEINFO_TYPEID")]
        public string BaseInfoTypeId { get; set; }

        [Column("TABLE_NAME")]
        public string TableName { get; set; }

        [Column("IS_SHOW")]
        public int IsShow { get; set; }

        [Column("SORT_NO")]
        public int SortNo { get; set; }

        [Column("UPDATE_TIME")]
        public DateTime UpdateTime { get; set; }

        [Column("REMARK")]
        public string Remark { get; set; }
    }
}