using NetApp.Models.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetApp.Common.Abstractions
{
    public interface ITreeRepo<T> : IListRepo<T> where T : ITreeNode<T>
    {
        Task<IList<T>> GetAllChildren(string id);
    }
}