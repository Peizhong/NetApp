using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetApp.Entities.Avmt
{
    [Table("DM_CLASSIFY_TECHPARAM")]
    public class ClassifyTechparamConfig
    {
        [Key]
        public string Id { get; set; }

        [Column("CLASSIFY_ID")]
        public string ClassifyId { get; set; }

        [Column("TECHPARAM_ID")]
        public string TechparamId { get; set; }

        [Column("COLUMN_NAME")]
        public string ColumnName { get; set; }

        [Column("DATA_TYPE")]
        public int DataType { get; set; }

        /// <summary>
        /// 1: Yes
        /// </summary>
        [Column("IS_SHOW")]
        public int IsDisplay { get; set; }

        [Column("SORT_NO")]
        public int SortNo { get; set; }
    }

    [Table("DM_TECHPARAM")]
    public class TechparamConfig
    {
        [Key]
        public string Id { get; set; }

        [Column("TECHPARAM_NAME")]
        public string TechparamName { get; set; }

        [Column("DATA_LENGTH")]
        public int DataLength { get; set; }

        [Column("DATA_PRECISION")]
        public int DataPrecision { get; set; }
    }

    [Table("DM_TECHPARAM_DICT")]
    public class TechparamDictConfig
    {
        [Key]
        public string Id { get; set; }

        [Column("CLASSIFY_TECHPARAM_ID")]
        public string ClassifyTechparamId { get; set; }

        [Column("TECHPARAM_VALUE")]
        public string DictionaryValue { get; set; }
    }

    public class TechInfoConfig
    {
        public string Id { get; set; }
        
        public string TechparamName { get; set; }

        public string ColumnName { get; set; }

        public int DataType { get; set; }

        public int DataLength { get; set; }

        public int DataPrecision { get; set; }

        public int SortNo { get; set; }

        public IList<TechparamDictConfig> TechinfoDict { get; set; }
    }
}