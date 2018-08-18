using System;
using System.Collections.Generic;
using System.Text;

namespace NetApp.Entities.Interfaces
{
    public interface ITreeNode<T> : IQuery where T : IQuery
    {
        string ParentId { get; set; }

        T Parent { get; set; }

        string FullPath { get; set; }

        ICollection<T> Children { get; }
    }
}