using System;
using System.Collections.Generic;
using System.Text;
using NetApp.Repository;

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
            var classifies = repo.GetAllClassifies();
            DateTime start = DateTime.Now;
            foreach (var c in classifies)
            {
                var bconfig = repo.GetBasicInfoConfigsWithCache(c.Id);
                foreach (var b in bconfig)
                {
                    ;
                }
            }

            Console.WriteLine($"took {DateTime.Now - start} to query all baseinfoconfg");
            
            start = DateTime.Now;
            foreach (var c in classifies)
            {
                var bconfig = repo.GetBasicInfoConfigsWithCache2(c.Id);
                foreach (var b in bconfig)
                {
                    ;
                }
            }
            Console.WriteLine($"took {DateTime.Now - start} to query all cache baseinfoconfg");
        }
    }
}