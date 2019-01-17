using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetApp.CeleryTask.Models
{
    [ProtoContract]
    public class RemoteCTask
    {
        [ProtoMember(1)]
        public string Id { get; set; }

        [ProtoMember(2)]
        public string TaskName { get; set; }

        [ProtoMember(3)]
        public DateTime CreateTime { get; set; }

        [ProtoMember(4)]
        public string ParamsJSON { get; set; }

        public Dictionary<string, string> Params
        {
            get
            {
                var dict = new Dictionary<string, string>();
                var pureJSON = ParamsJSON?.Trim();
                //accept json object only
                if (!string.IsNullOrEmpty(pureJSON) && pureJSON.StartsWith("{") && pureJSON.EndsWith("}"))
                {
                    JObject obj = JObject.Parse(pureJSON);
                    foreach (var child in obj.Children())
                    {
                        dict.Add(child.Path, obj[child.Path].Value<string>());
                    }
                }
                return dict;
            }
        }
    }
}
