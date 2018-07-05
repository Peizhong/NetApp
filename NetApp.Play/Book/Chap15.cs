using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net;

namespace NetApp.Play.Book
{ 
    [LastModified("2018-01-01","learn stuff")]
    public class Chap15
    {
        public string Mimi { get; set; }

        
        public async void DoSomething()
        {
            await GreetingAsync("mimi");
        }

        /// <summary>
        /// 线程池中调用线程
        /// </summary>
        /// <param name="info"></param>
        public static void TraceThreadAndTask(string info)
        {
            string taskInfo = Task.CurrentId == null ? "no task" : $"task{Task.CurrentId}";
            Console.WriteLine($"{info} in thread{ Thread.CurrentThread.ManagedThreadId}" + $" and {taskInfo}");
        }

        public static string Greeting(string name)
        {
            TraceThreadAndTask($"running {nameof(Greeting)}");
            Task.Delay(3000).Wait();
            return $"hello, {name}";
        }

        public static Task<string> GreetingAsync(string name) => Task.Run(() =>
             {
                 TraceThreadAndTask($"running {nameof(GreetingAsync)}");
                 return Greeting(name);
             });

        public static async Task ConvertAsync()
        {
            HttpWebRequest request = WebRequest.Create("http://www.baidu.com") as HttpWebRequest;
            using (WebResponse response = await Task.Factory.FromAsync(request.BeginGetResponse(null, null), request.EndGetResponse))
            {

            }
        }
    }

    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method | AttributeTargets.Constructor, AllowMultiple = true, Inherited = false)]
    public class LastModifiedAttribute : Attribute
    {
        public DateTime DateModified { get; }

        public string Changes { get; }

        public string Issues { get; set }

        public LastModifiedAttribute(string dateModified, string changes)
        {
            DateModified = DateTime.Parse(dateModified);
            Changes = changes;
        }
    }

    [AttributeUsage(AttributeTargets.Assembly)]
    public class SupportWhatsNewAttribute : Attribute
    {

    }
}