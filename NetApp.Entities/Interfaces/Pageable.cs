using System;
using System.Collections.Generic;
using System.Text;

namespace NetApp.Entities.Interfaces
{
    public interface IPageable
    {
        uint StartIndex { get; }
        uint PageSize { get; }
        bool Reverse { get; }
    }

    public class Pageable
    {
        public uint StartIndex { get; set; }

        public uint PageSize { get; set; }

        public bool Reverse { get; set; }
    }
}