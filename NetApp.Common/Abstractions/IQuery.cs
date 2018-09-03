using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;

namespace NetApp.Common.Abstractions
{
    public interface IQuery<T> where T : IBase
    {
        Expression<Func<T, bool>> Filter { get; set; }
    }
}
