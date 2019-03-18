using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;

namespace NetApp.Play.Book
{
    /// <summary>
    /// 性能测试
    /// n个连接，m此请求
    /// </summary>
    public class Benchmark
    {
        private static readonly ConcurrentDictionary<string, string> _cache = new ConcurrentDictionary<string, string>();
        BlockingCollection<string> blockingCollection = new BlockingCollection<string>();
        private static readonly string _key = Guid.NewGuid().ToString();
        private static volatile int _hit = 0;

        private async Task<List<string>> apiTest()
        {
            List<string> result = null;
            if (_cache.TryGetValue(_key, out var raw))
            {
                Interlocked.Increment(ref _hit);
            }
            else
            {
                await Task.Delay(2000);
                var data = Enumerable.Range(0, 3000).Select(n => Guid.NewGuid().ToString()).ToList();
                raw = JsonConvert.SerializeObject(data);
                // Console.WriteLine($"str size {raw.Length}");
                _cache.TryAdd(_key, raw);
            }
            result = JsonConvert.DeserializeObject<List<string>>(raw);
            return result;
        }

        /// <summary>
        /// 性能测试
        /// </summary>
        /// <param name="action">任务</param>
        /// <param name="connection">线程数</param>
        /// <param name="requests">任务数</param>
        public void Attack(Action action, int connection, int requests)
        {
            // 临界
            if (action == null || connection < 1 || requests < 1)
                return;
            int size = 0;
            action = async () =>
            {
                var data = await apiTest();
                size = data.Sum(d => d.Length);
            };
            DateTime start = DateTime.Now;
            Console.WriteLine($"Attack start");
            ConcurrentQueue<int> queue = new ConcurrentQueue<int>(Enumerable.Range(0, requests));
            Parallel.For(0, connection, n =>
            {
                while (queue.TryDequeue(out int index))
                {
                    // Console.WriteLine($"do {n} at {Thread.CurrentThread.ManagedThreadId} / {Task.CurrentId}");
                    action?.Invoke();
                }
            });
            Console.WriteLine($"Attack -c {connection} -n {requests} elapse {DateTime.Now - start} hit {_hit} strsize {size}");
        }

        public async Task BC_AddTakeCompleteAdding()
        {
            using (BlockingCollection<int> bc = new BlockingCollection<int>())
            {

                // Spin up a Task to populate the BlockingCollection 
                using (Task t1 = Task.Factory.StartNew(() =>
                {
                    bc.Add(1);
                    bc.Add(2);
                    bc.Add(3);
                    bc.CompleteAdding();
                }))
                {
                    // Spin up a Task to consume the BlockingCollection
                    using (Task t2 = Task.Factory.StartNew(() =>
                    {
                        try
                        {
                            // Consume consume the BlockingCollection
                            while (true) Console.WriteLine(bc.Take());
                        }
                        catch (InvalidOperationException)
                        {
                            // An InvalidOperationException means that Take() was called on a completed collection
                            Console.WriteLine("That's All!");
                        }
                    }))
                    await Task.WhenAll(t1, t2);
                }
            }
        }

        public void BC_GetConsumingEnumerable()
        {
            using (BlockingCollection<int> bc = new BlockingCollection<int>())
            {

                // Kick off a producer task
                Task.Factory.StartNew(async () =>
                {
                    for (int i = 0; i < 10; i++)
                    {
                        bc.Add(i);
                        await Task.Delay(100); // sleep 100 ms between adds
                    }

                    // Need to do this to keep foreach below from hanging
                    bc.CompleteAdding();
                });

                // Now consume the blocking collection with foreach.
                // Use bc.GetConsumingEnumerable() instead of just bc because the
                // former will block waiting for completion and the latter will
                // simply take a snapshot of the current state of the underlying collection.
                foreach (var item in bc.GetConsumingEnumerable())
                {
                    Console.WriteLine(item);
                }
            }
        }
    }
}
