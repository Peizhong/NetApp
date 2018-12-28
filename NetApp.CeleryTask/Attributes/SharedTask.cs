using System;
using System.Collections.Generic;
using System.Text;

namespace NetApp.CeleryTask.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class SharedTaskAttribute : Attribute
    {
        public string TaskName { get; set; }

        public int Interval { get; set; }
    }
}
