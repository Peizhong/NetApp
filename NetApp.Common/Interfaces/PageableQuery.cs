using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;

namespace NetApp.Common.Interfaces
{
    /// <summary>
    /// to repo
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageableQuery<T> : IQuery<T>, IPageable where T : IBase
    {
        public int StartIndex { get; set; }

        public int PageSize { get; set; }

        public Expression<Func<T, bool>> Filter { get; set; }

        public Expression<Func<T, object>> Sort { get; set; }

        public bool Reverse { get; set; }
    }
}