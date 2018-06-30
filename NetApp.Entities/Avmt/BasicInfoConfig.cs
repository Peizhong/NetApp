using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace NetApp.Entities.Avmt
{
    [Table("DM_BASEINFO_CONFIG")]
    public class BasicInfoConfig
    {
        [JsonProperty("id")]
        [Key]
        public string Id { get; set; }

        [JsonProperty("baseinfoTypeId")]
        [Column("BASEINFO_TYPEID")]
        public string BaseInfoTypeId { get; set; }

        /// <summary>
        /// 对象类型，1功能位置；2设备；3部件
        /// </summary>
        [JsonProperty("objectType")]
        [Column("OBJECT_TYPE")]
        public int ObjectType { set; get; }

        /// <summary>
        /// 字段中文名
        /// </summary>
        [JsonProperty("fieldName")]
        [Column("FIELD_NAME")]
        public string FieldName { set; get; }

        /// <summary>
        /// 字段英文名
        /// </summary>
        [JsonProperty("fieldAlias")]
        [Column("FIELD_ALIAS")]
        public string FieldAlias { set; get; }

        /// <summary>
        /// 对应的数据库字段名
        /// </summary>
        [JsonProperty("fieldColumn")]
        [Column("FIELD_COLUMN")]
        public string FieldColumn { set; get; }

        /// <summary>
        /// 1:字符，2数字，3日期
        /// </summary>
        [JsonProperty("dataType")]
        [Column("DATA_TYPE")]
        public int DataType { set; get; }

        /// <summary>
        /// 字段长度
        /// </summary>
        [JsonProperty("dataLength")]
        [Column("DATA_LENGTH")]
        public int DataLength { set; get; }

        /// <summary>
        /// 字段精度（number）
        /// </summary>
        [JsonProperty("dataPrecision")]
        [Column("DATA_PRECISION")]
        public int DataPrecision { set; get; }

        /// <summary>
        /// 是否显示，1显示；0不显示，默认为0
        /// </summary>
        [JsonProperty("isDisplay")]
        [Column("IS_DISPLAY")]
        public int IsDisplay { set; get; }

        [JsonProperty("isVendorFill")]
        [Column("IS_VENDOR_FILL")]
        public int IsVendorFill { get; set; }

        /// <summary>
        /// 显示顺序
        /// </summary>
        [JsonProperty("sortNo")]
        [Column("SORT_NO")]
        public int SortNo { set; get; }

        /// <summary>
        /// 是否只读，1只读；0非只读，默认为0
        /// </summary>
        [JsonProperty("isOnlyread")]
        [Column("IS_ONLYREAD")]
        public int IsReadOnly { set; get; }

        /// <summary>
        /// 是否必填，1必填；0非必填，默认为0
        /// </summary>
        [JsonProperty("isNullable")]
        [Column("IS_NULLABLE")]
        public int IsMandatory { set; get; }

        /// <summary>
        /// 字典ID
        /// </summary>
        [JsonProperty("dictionaryId")]
        [Column("DICTIONARY_ID")]
        public int DictionaryId { set; get; }

        /// <summary>
        /// 是否查询特殊值表，1是；0否
        /// </summary>
        [JsonProperty("isSpecial")]
        [Column("IS_SPECIAL")]
        public int IsSpecial { set; get; }

        /// <summary>
        /// 主配网标识
        /// </summary>
        [JsonProperty("powerGridFlag")]
        [Column("POWER_GRID_FLAG")]
        public int PowerGridFlag { set; get; }

        /// <summary>
        /// 流程阶段.1设计阶段完善,2物资供应商阶段,3施工阶段完善,4运行阶段
        /// </summary>
        [JsonProperty("flowerStep")]
        [Column("FLOWER_STEP")]
        public int FlowerStep { set; get; }

        [JsonIgnore]
        [NotMapped]
        public IList<BasicInfoDictConfig> BaseinfoDict { get; set; }
    }

    [Table("DM_BASEINFO_DICT")]
    public class BasicInfoDictConfig
    {
        [JsonProperty("id")]
        [Key]
        public string Id { get; set; }

        /// <summary>
        /// 字典ID
        /// </summary>
        [JsonProperty("dictionaryId")]
        [Column("DICTIONARY_ID")]
        public int DictionaryId { get; set; }

        /// <summary>
        /// 字典取值标识
        /// </summary>
        [JsonProperty("keyOrValue")]
        [Column("KEY_OR_VALUE")]
        public int KeyOrValue { get; set; }

        /// <summary>
        /// 基本信息数据字典值
        /// </summary>
        [JsonProperty("dictionaryValue")]
        [Column("DICTIONARY_VALUE")]
        public string BaseinfoDictValue { get; set; }

        /// <summary>
        /// 基本信息数据字典键
        /// </summary>
        [JsonProperty("dictionaryKey")]
        [Column("DICTIONARY_KEY")]
        public int BaseinfoDictKey { get; set; }
        /// <summary>
        /// 字典值排序
        /// </summary>
        [JsonProperty("sortNo")]
        [Column("SORT_NO")]
        public int SortNo { get; set; }
    }
}