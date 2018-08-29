using System;
using System.Collections.Generic;
using System.Text;

namespace NetApp.Common.Interfaces
{
    public interface ITreeNode : IBase
    {
        string ParentId { get; set; }

        string FullPath { get; set; }

        double? SortNo { get; set; }
    }

    public interface ITreeNode<T> : ITreeNode where T : IBase
    {
        T Parent { get; set; }

        ICollection<T> Children { get; set; }
    }
}