using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NetApp.Repository;
using NetApp.Entities.Avmt;

namespace NetApp.Play.Book
{
    /// <summary>
    /// linq: language integrated query
    /// parallel linq
    /// deferred query excution: query runs only when the items are iterated
    /// expression trees
    /// </summary>
    public class Chap12
    {
        public void DoQuery()
        {
            var repo = new SQBaseInfoRepo();

            Console.WriteLine($"took {DateTime.Now - start} to query all cache baseinfoconfg");
        }

        public void DoGroup()
        {
            var repo = new SQBaseInfoRepo();

        }
    }
}