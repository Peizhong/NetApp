using System;
using System.Collections.Generic;
using System.Text;

namespace NetApp.Entities.Avmt
{
    public class TechparamConfig
    {
        public string Id { get; set; }

        public string ClassifyId { get; set; }

        public string FieldName { get; set; }

        public string FieldColumn { get; set; }

        public int DataType { get; set; }

        public int DataLength { get; set; }

        public int DataPrecision { get; set; }

        /// <summary>
        /// 1: Yes
        /// </summary>
        public int IsDisplay { get; set; }

        public int SortNo { get; set; }
    }

    public class TechparamDictConfig
    {
        public string Id { get; set; }
        public string TechparamId { get; set; }
        public string DictionaryValue { get; set; }
        public int SortNo { get; set; }
    }
}