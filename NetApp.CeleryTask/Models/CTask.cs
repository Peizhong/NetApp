using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetApp.CeleryTask.Models
{
    [ProtoContract]
    public class CTask
    {
        [ProtoMember(1)]
        public string TaskName { get; set; }

        [ProtoMember(2)]
        public string TypeName { get; set; }

        [ProtoMember(3)]
        public string MethodName { get; set; }
        
        public IList<object> Params { get; set; }
    }
}
