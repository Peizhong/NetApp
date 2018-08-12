using System;
using System.Collections.Generic;
using System.Text;

namespace NetApp.Entities.Interfaces
{
    public interface ITreeNode : IQuery
    {
        string ParentId { get; set; }

        ITreeNode Parent { get; set; }

        string FullPath { get; set; }

        IEnumerable<ITreeNode> Children { get; }
    }
}