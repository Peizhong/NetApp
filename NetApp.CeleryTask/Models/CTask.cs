using System;
using System.Collections.Generic;
using System.Text;

namespace NetApp.CeleryTask.Models
{
    public class CTask
    {
        public string Name { get; set; }

        public object Params { get; set; }
    }
}
