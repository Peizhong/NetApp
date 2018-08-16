using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using NetApp.Entities.Interfaces;

namespace NetApp.Repository.Interfaces
{
    public interface IListRepo<T> where T : IQuery
    {
        Task<T> FindAsync(string id);

        Task<T> GetAsync(Expression expression);

        Task<IList<T>> GetListAsync(Expression expression);

        Task UpdateAsync(T value);

        Task UpdateListAsync(IEnumerable<T> values);
    }
}