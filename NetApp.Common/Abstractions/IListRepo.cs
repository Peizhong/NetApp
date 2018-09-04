using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetApp.Common.Abstractions
{
    public interface IListRepo<T> where T : IBase
    {
        Task<T> FindAsync(string id);

        Task<T> GetAsync(IQuery<T> query);

        Task<PageableQueryResult<T>> GetListAsync(PageableQuery<T> query);

        Task UpdateAsync(T value, RepoOption option = null);

        Task UpdateListAsync(IEnumerable<T> values, RepoOption option = null);
    }
}