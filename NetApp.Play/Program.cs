using System;
using System.Linq;
using System.Buffers;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Net.Http;

namespace NetApp.Play
{
    class Program
    {
        class A
        {
            protected int vv = 0;

            public A()
            {
                Console.WriteLine("hello a");
                Stuff();
            }

            public A(int b)
            {
                vv = b;
            }

            public virtual void Stuff()
            {
                Console.WriteLine("hello a, doing something");
            }
        }

        class B : A
        {
            string mimi = null;
            public B()
                :base(1)
            {
                
                mimi = "abc";
                Console.WriteLine("hello b");
            }
            
            public override void Stuff()
            {
                int x = vv;
                mimi.ToString();
                Console.WriteLine("hello b, doing something");
            }
        }

        static void Main(string[] args)
        {
            var v = (6503154).ToString();
            var sp = v.Split('.');
            var i = string.Concat(sp[0].Take(3));
            var d = "0";
            if (sp.Length > 1)
            {
                d = string.Concat(sp[1].Take(2));
            }
            var cv = decimal.Parse($"{i}.{d}");


            Task.Run(async () =>
            {
                HttpClient c = new HttpClient(new HttpClientHandler
                {
                    Proxy = null,
                    UseProxy = false
                });
                while (true)
                {
                    var m = await c.GetAsync("http://localhost:8000/api/mallservice/");
                    var s = m.Content.ReadAsStringAsync();
                }

            }).Wait();

            B b = new B();

            EncodingProvider provider = CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(provider);

            //EFTest();

            var chap = new Book.Chap23();
            //var task = chap.TcpHttpClient();
            Console.WriteLine("now getting something...");
            //task.Wait();
            Console.WriteLine("done");
            Console.ReadKey();
        }
    }
}