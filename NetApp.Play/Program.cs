using System;
using System.Linq;
using System.Buffers;
using System.Text;

namespace NetApp.Play
{
    class Program
    {
        static void Main(string[] args)
        {
            EncodingProvider provider = CodePagesEncodingProvider.Instance;
            Encoding.RegisterProvider(provider);

            var chap = new Book.Chap23();
            var task = chap.TcpHttpClient();
            Console.WriteLine("now getting something...");
            task.Wait();
            Console.WriteLine("done");
            Console.ReadKey();
        }
    }
}