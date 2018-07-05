using System;
using System.Linq;
using System.Buffers;

namespace NetApp.Play
{
    public class TestData
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public void Func(int a)
        {

        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var chap = new Book.Chap15();
            chap.DoSomething();
            var word = new Portal.Words();
            word.GetWordDefine("王");
            Console.WriteLine("done");
            Console.ReadKey();
        }

        static void learnArray()
        {
            //一维数组
            int[] numbers = { 1, 3, 4, 5 };
            var numbers2 = new int[] { 1, 3, 4, 5 };
            //多维数组
            int[,,] array1 = new int[4, 3, 1];
            int[,] array2D = new int[,] { { 1, 2 }, { 3, 4 }, { 5, 6 }, { 7, 8 } };
            //交错数组:数组的数组
            int[][] jaggedArray = new int[3][];
            jaggedArray[0] = new int[] { 1, 3, 5, 7, 9 };
            jaggedArray[1] = new int[] { 0, 2, 4, 6 };
            jaggedArray[2] = new int[] { 11, 22 };
            //jaggedArray还是一维的
            Console.WriteLine($"{array1.Rank} {jaggedArray.Rank}");
        }

        static void learnDynamic()
        {
            var ex = new TestData();
            dynamic dyEx = new TestData();
            dyEx.mmimi = 123;
            //ex.learnDynamic(1, 3, 4);
            dyEx.learnDynamic(1, 3, 4);
            dyEx.dome(1, 3);
        }

        static void learnSpan()
        {
            int[] arr1 = { 1, 2, 3, 4, 5, 6, 7, 8 };
            var span1 = new Span<int>(arr1);
            span1.Clear();
            span1.Fill(43);
            arr1[1] = 2;
            span1[2] = 1;
            var span2 = span1.Slice(start: 2, length: 3);
            ReadOnlySpan<int> readSpan = new ReadOnlySpan<int>(arr1);

            ArrayPool<int> customPool = ArrayPool<int>.Create(maxArrayLength: 1024 * 1024, maxArraysPerBucket: 3);

            foreach (var n in Enumerable.Range(1, 10))
            {
                int length = n << 2;
                int[] arr = customPool.Rent(length);
                var span3 = new Span<int>(arr);
                span3.Fill(n);
                Console.WriteLine($"request {length} and get {arr.Length}");
                customPool.Return(arr, clearArray: false);
            }
        }
    }
}