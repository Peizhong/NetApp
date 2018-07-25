using System;
using System.Linq;
using System.Buffers;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace NetApp.Play
{
    class Program
    {
        static void Main(string[] args)
        {
            EncodingProvider provider = CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(provider);

            EFTest();

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