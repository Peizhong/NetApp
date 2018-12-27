using NetApp.CeleryTask.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetApp.Play.Utils
{
    public class Demo
    {
        public int Num { get; set; }

        public string Str { get; set; }
    }

    public class CeleryWorker
    {
        [SharedTask(Interval = 1000)]
        public void HelloString(string world)
        {
        }

        [SharedTask]
        public string HelloMulit(int num, string str)
        {
            return $"int is {num}, str is {str}";
        }

        [SharedTask]
        public void HelloClass(Demo demo)
        {

        }
    }
}
