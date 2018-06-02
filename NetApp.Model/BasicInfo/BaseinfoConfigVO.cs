using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace NetApp.Model.BasicInfo
{
    public class BaseinfoConfigVO
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("fieldName")]
        public string FieldName { get; set; }

        [JsonProperty("fieldColumn")]
        public string FieldColumn { get; set; }
    }
}
