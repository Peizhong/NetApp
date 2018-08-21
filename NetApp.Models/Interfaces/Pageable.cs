using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace NetApp.Models.Interfaces
{
    public interface IPageable<T>
    {
        int StartIndex { get; set; }
        int PageSize { get; set; }
        bool Reverse { get; set; }

        //Expression<Func<T, object>> Sort { get; }
    }
}