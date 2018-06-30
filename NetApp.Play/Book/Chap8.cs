using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetApp.Play.Book
{
    public class Chap8
    {
        delegate string CheckData();

        string hello()
        {
            return "1";
        }

        string hello2(int m)
        {
            return "1";
        }


        public void DoSomething()
        {
            int x = 40;
            int y = 50;
            int[] xx = { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            (xx[1], xx[0]) = (xx[0], xx[1]);
            CheckData firstMethod = new CheckData(() => hello2(1));
            var v = firstMethod();
            Random random = new Random(43);
            var peoples = Enumerable.Range(1, 10).Select(n => new People
            {
                Name = n.ToString(),
                Age = random.Next(100),
            }).ToList();
            int count = BubbleSorted(peoples, People.Comparison);
            Console.WriteLine($"run {count} times");
            Console.WriteLine(string.Join(",", peoples.Select(p => p.Age)));
        }

        public int BubbleSorted<T>(IList<T> array, Func<T, T, bool> comparison)
        {
            int runtime = 0;
            bool swaped = true;
            do
            {
                swaped = false;
                for (int i = 0; i < array.Count - 1; i++)
                {
                    runtime++;
                    if (comparison(array[i + 1], array[i]))
                    {
                        (array[i], array[i + 1]) = (array[i + 1], array[i]);
                        swaped = true;
                    }
                }
            } while (swaped);
            return runtime;
        }

        struct People
        {
            public string Name { get; set; }

            public int Age { get; set; }

            public static bool Comparison(People a, People b) => a.Age > b.Age;
        }
    }
}
