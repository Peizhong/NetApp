using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace NetApp.Play.Book
{
    public class Chap13
    {
        public void DoSomething()
        {
            //tuples
            var t = (1, 3, "mm");
            var m = from n in Enumerable.Range(0, 100)
                    select (index: n, value: n * n);
            var c = new { index = 1, value = 2 };
        }
    }
}
