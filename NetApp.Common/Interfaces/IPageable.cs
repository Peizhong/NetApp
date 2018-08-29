using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace NetApp.Common.Interfaces
{
    /// <summary>
    /// from the url
    /// </summary>
    public interface IPageable
    {
        int StartIndex { get; set; }

        int PageSize { get; set; }
    }
}