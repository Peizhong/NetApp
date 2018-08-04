using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetApp.CLR
{
    class Program
    {
        static void Main(string[] args)
        {
            int v = 123;
            SimpleOne(v);
            Console.ReadKey();
        }

        static void SimpleOne(int v)
        {
            //Evaluation Stack
            int a = 1;//ldc.i4.1
            int b = 2;
            //Record Frame
            int c =  b+a;//stloc,ldloc
            Console.Write(c);
        }
    }
}
