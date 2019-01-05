using System;
using System.Collections.Generic;
using System.Text;

namespace NetApp.CeleryTask.Models
{
    public class CTask
    {
        public string Id { get; set; }

        /// <summary>
        /// default is methodname
        /// </summary>
        public string TaskName { get; set; }
        
        public string TypeName { get; set; }
        
        public string MethodName { get; set; }

        public List<CTaskParam> Params { get; set; }
    }

    public class CTaskParam
    {
        public string TypeName { get; set; }

        public string ParamName { get; set; }

        public object Value { get; set; }
    }
}
