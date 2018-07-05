using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NetApp.Entities.Avmt;

namespace NetApp.Play.Book
{
    /// <summary>
    /// Generics
    /// </summary>
    public class Chap5
    {
        public void DoSomething()
        {
            Nullable<int> x;
            x = 4;
            int b = (int)x;


            var array = new System.Collections.ArrayList();
            var r = new Entities.Avmt.BasicInfoConfig();
            var basicData = new[] {
            new {
                type= typeof(Entities.Avmt.BasicInfoConfig), table="DM_BASIC_CONFIG",
            },
            new {
                type= typeof(Entities.Avmt.BasicInfoDictConfig), table="DM_BASIC_DICT",
            },
            };
            foreach (var tb in basicData)
            {
                var n = tb.type.Name;
                var list = download(Activator.CreateInstance(tb.type), tb.table);
            }
            var es = download(new BasicInfoConfig(), "ads");

            var cf = new Classify();
            var classifyNodes = new LinkNode<Classify>(cf);
            var f = classifyNodes.Where(c => !string.IsNullOrEmpty(c.FullName)).ToList();
        }

        public List<T> download<T>(T a, string tableName)
        {
            return new List<T>();
        }

    }

    public class LinkNode<T> : IEnumerable<T>
    {
        public LinkNode(T value)
        {
            Value = value;
        }

        public T Value { get; }

        public LinkNode<T> PreviousNode { get; internal set; }
        public LinkNode<T> NextNode { get; internal set; }

        public IEnumerator<T> GetEnumerator()
        {
            var current = this;
            while (current != null)
            {
                yield return current.Value;
                current = current.NextNode;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    /// <summary>
    /// where define T to implement the interface IDocument
    /// </summary>
    /// <typeparam name="TDocument"></typeparam>
    public class DocumentManager<TDocument> where TDocument : IDocumnet
    {
        private readonly Queue<TDocument> _documents = new Queue<TDocument>();
        private readonly object _lockQueue = new object();

        public int Add(TDocument value)
        {
            lock (_lockQueue)
            {
                _documents.Append(value);
            }
            return _documents.Count;
        }

        public bool IsQueueable => _documents.Count > 0;

        public TDocument GetDocumnet()
        {
            TDocument doc = default(TDocument);
            lock (_lockQueue)
            {
                doc = _documents.Dequeue();
            }
            return doc;
        }

        public void DisplayAllDocument()
        {
            foreach (TDocument doc in _documents)
            {
                Console.WriteLine(doc.Title);
            }
        }
    }

    public interface IDocumnet
    {
        string Title { get; }
        string Content { get; }
    }

    public class Document : IDocumnet
    {
        public Document(string title, string content)
        {
            Title = title;
            Content = content;
        }

        public string Title { get; }
        public string Content { get; }
    }

    public struct Nullable<T> where T : struct
    {
        private bool _hasValue;
        public bool HasValue => _hasValue;

        private T _value;
        public T Value
        {
            get
            {
                if (!_hasValue)
                {
                    throw new InvalidOperationException("no value");
                }
                return _value;
            }
        }

        public Nullable(T value)
        {
            _value = value;
            _hasValue = true;
        }

        /// <summary>
        /// 显示转换 Nullable<T>转T
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator T(Nullable<T> value) => value.Value;

        /// <summary>
        /// 隐式转换
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator Nullable<T>(T value) => new Nullable<T>(value);
    }
}