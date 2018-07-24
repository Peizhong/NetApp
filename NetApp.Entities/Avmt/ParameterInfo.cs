using System;
using System.Collections.Generic;
using System.Text;

namespace NetApp.Entities.Avmt
{
    public class ParameterInfo
    {
        public string DisplayName { get; set; }

        public string ColumnName { get; set; }

        public string Value { get; set; }

        public int DataType { get; set; }

        /// <summary>
        /// 1.基本信息 2.技术参数
        /// </summary>
        public int DataSource { get; set; }

        public int SortNo { get; set; }
    }
}
