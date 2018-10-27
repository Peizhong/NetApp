using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using NetApp.Models.Abstractions;

namespace NetApp.Common.Abstractions
{
    /// <summary>
    /// to repo
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PageableQuery<T> : Pageable, IQuery<T> where T : IBase
    {
        public Expression<Func<T, bool>> Filter { get; set; }

        public Expression<Func<T, object>> Sort { get; set; }

        public bool Reverse { get; set; }
    }
}