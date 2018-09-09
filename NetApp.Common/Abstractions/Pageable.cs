using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace NetApp.Common.Abstractions
{
    /// <summary>
    /// from the url
    /// </summary>
    public class Pageable
    {
        public int StartIndex { get; set; }

        public int PageSize { get; set; }
    }
}