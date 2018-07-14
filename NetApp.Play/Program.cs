using System;
using System.Linq;
using System.Buffers;

namespace NetApp.Play
{
    class Program
    {
        static void Main(string[] args)
        {
            var chap = new Book.Chap21();
            chap.DoSomething();
            Console.WriteLine("done");
            Console.ReadKey();
        }
    }
}