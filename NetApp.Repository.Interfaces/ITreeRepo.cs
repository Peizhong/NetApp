using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NetApp.Entities.Interfaces;

namespace NetApp.Repository.Interfaces
{
    public interface ITreeRepo<T> : IListRepo<T> where T : ITreeNode
    {
        Task<IList<T>> GetRoot();

        Task<IList<T>> GetChildren(string id);

        Task<IList<T>> GetAllChildren(string id);
    }
}
