using System;
using System.Collections.Generic;
using System.Text;

namespace NetApp.Common.Abstractions
{
    public interface IBase
    {
        string Id { get; set; }

        string Name { get; set; }

        /// <summary>
        /// 2:deleted
        /// </summary>
        int DataStatus { get; set; }
    }
}