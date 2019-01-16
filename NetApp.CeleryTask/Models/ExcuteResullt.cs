using System;
using System.Collections.Generic;
using System.Text;

namespace NetApp.CeleryTask.Models
{
    public class ExcuteResult
    {
        public string Code { get; set; }

        public string Message { get; set; }
    }

    public class TaskExcuteResult : ExcuteResult
    {
        public object Result { get; set; }
    }
}
