using System;
using System.Collections.Generic;
using System.Text;

namespace NetApp.CeleryTask.Models
{
    public class CTask
    {
        public string Assembly { get; set; }

        public Type Type { get; set; }

        public string Name { get; set; }

        public IList<object> Params { get; set; }
    }
}
