using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetApp.Play.Book
{
    //list
    //queues: first in first out
    //stack:last in first out
    //,linked list
    //performance: list.insert(O(1) or O(n) if reallocate)
    public class Chap10
    {
        public void DoSomething()
        {
            var list = Enumerable.Range(1, 100).Select(n => new DV
            {
                Key = n.ToString(),
                Define = $"this is {n}"
            });
            var look = list.ToLookup(r => r.Key);
            foreach(var l in look["bd"])
            {

            }

        }
        public class DV
        {
            public string Key { get; set; }
            public string Define { get; set; }
        }
    }
}
