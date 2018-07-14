using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace NetApp.Play.Book
{
    class Chap21
    {
        /*
         task and parallel
         */
        public void DoSomething()
        {
            var target = GetUpPipeline();
            target.Post(".");
        }

        /// <summary>
        /// data flow in parallel
        /// </summary>
        public void Do_DataFlow()
        {
            var processInput = new ActionBlock<string>(s =>
            {
                Console.WriteLine($"you said {s} (done by {Thread.CurrentThread.ManagedThreadId})");
            });
            while (true)
            {
                string input = Console.ReadLine();
                if (string.Compare(input, "exit", ignoreCase: true) == 0)
                {
                    break;
                }
                processInput.Post(input);
            }
        }


        BufferBlock<string> bufferBlock = new BufferBlock<string>();

        public void Producer()
        {
            while (true)
            {
                string input = Console.ReadLine();
                if (string.Compare(input, "exit", ignoreCase: true) == 0)
                {
                    return;
                }
                bufferBlock.Post(input);
                Console.WriteLine($"post {input} (from {Thread.CurrentThread.ManagedThreadId})");
            }
        }

        public async Task ConsumerAsync()
        {
            while (true)
            {
                var input = await bufferBlock.ReceiveAsync();
                Console.WriteLine($"recive {input} (from {Thread.CurrentThread.ManagedThreadId})");
            }
        }

        public IEnumerable<string> GetFileNames(string path)
        {
            foreach (var fileName in Directory.EnumerateFiles(path, "*.cs"))
            {
                Console.WriteLine($"GetFileNames {fileName} (from {Thread.CurrentThread.ManagedThreadId})");
                yield return fileName;
            }
        }

        public IEnumerable<string> GetLines(IEnumerable<string> fileNames)
        {
            foreach (var file in fileNames)
            {
                using (FileStream f = File.Open(file, FileMode.Open))
                {
                    var reader = new StreamReader(f);
                    string line = null;
                    while ((line = reader.ReadLine()) != null)
                    {
                        Console.WriteLine($"GetLines {line} (from {Thread.CurrentThread.ManagedThreadId})");
                        //WriteLine($"LoadLines {line}");
                        yield return line;
                    }
                }
            }
        }

        public IEnumerable<string> GetWords(IEnumerable<string> lines)
        {
            foreach (var line in lines)
            {
                string[] words = line.Split(' ', ';', '(', ')', '{', '}', '.', ',');
                foreach (var word in words)
                {
                    if (!string.IsNullOrEmpty(word))
                    {
                        Console.WriteLine($"GetWords {word} (from {Thread.CurrentThread.ManagedThreadId})");
                        yield return word;
                    }
                }
            }
        }

        public ITargetBlock<string> GetUpPipeline()
        {
            var fileNamesForPath = new TransformBlock<string, IEnumerable<string>>(path => GetFileNames(path));
            var lineForFile = new TransformBlock<IEnumerable<string>, IEnumerable<string>>(file => GetLines(file));
            var wordForLine = new TransformBlock<IEnumerable<string>, IEnumerable<string>>(line => GetWords(line));
            var display = new ActionBlock<IEnumerable<string>>(coll =>
            {
                foreach (var s in coll)
                {
                    //Console.WriteLine(s);
                }
            });
            fileNamesForPath.LinkTo(lineForFile);
            lineForFile.LinkTo(wordForLine);
            wordForLine.LinkTo(display);
            return fileNamesForPath;
        }
    }
}
