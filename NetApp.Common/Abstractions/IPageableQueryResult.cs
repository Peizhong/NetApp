using System;
using System.Collections.Generic;
using System.Text;

namespace NetApp.Common.Abstractions
{
    public class PageableQueryResult<T> : IPageable
    {
        public IList<T> Items { get; set; }

        public int StartIndex { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int CurrentCount { get; set; }

        public string Host { get; set; }

        public string Message { get; set; }
    }
}