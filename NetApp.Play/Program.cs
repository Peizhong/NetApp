using System;
using System.Linq;
using System.Buffers;
using System.Text;
using Microsoft.EntityFrameworkCore;

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

        static async void EFTest()
        {
            var dbConfig = new DbContextOptionsBuilder<Repository.AvmtDbContext>();
            string connectionString = @"Data Source=avmt.db";
            dbConfig.UseSqlite(connectionString);
            using (var context = new Repository.AvmtDbContext(dbConfig.Options))
            {
                try
                {
                    var cc = await context.Cars.AsNoTracking().ToListAsync();
                    var baseinfo = await context.BasicinfoConfigs.AsNoTracking().Where(b => b.Id == "70").ToListAsync();
                    var good = baseinfo.Any(b => b.BaseinfoDict?.Any() == true);
                }
                catch (Exception ex)
                {
                    var m = ex.Message;
                }
            }
        }
    }
}