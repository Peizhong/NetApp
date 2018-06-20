using System;

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

    /// <summary>
    /// 泛型
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericList<T>
    {
        public void Add(T input)
        {
            ;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            learnArray();
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
    }
}
