using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NetApp.Models.Interfaces;

namespace NetApp.Repository
{
    public abstract class EFRepo
    {
        protected DbContext _absContext;

        protected async Task<T> getByIdAsync<T>(string id) where T : class
        {
            var result = await _absContext.Set<T>().FindAsync(id);
            return result;
        }

        protected async Task<T> getByQueryAsync<T>(IQuery<T> query) where T : class, IBase
        {
            T result = default(T);
            if (query?.Filter != null)
                result = await _absContext.Set<T>().FirstOrDefaultAsync(query.Filter);
            else
                result = await _absContext.Set<T>().FirstOrDefaultAsync();
            return result;
        }

        protected async Task<PageableQueryResult<T>> getListByQueryAsync<T>(PageableQuery<T> query) where T : class, IBase
        {
            var result = new PageableQueryResult<T>();
            try
            {
                IQueryable<T> todo = _absContext.Set<T>();
                if (query != null)
                {
                    result.StartIndex = query.StartIndex;
                    result.PageSize = query.PageSize;

                    if (query.Filter != null)
                        todo = todo.Where(query.Filter);
                }
                result.TotalCount = await todo.CountAsync();
                if (query != null)
                {
                    if (query.Sort != null)
                    {
                        if (query.Reverse)
                            todo = todo.OrderByDescending(query.Sort);
                        else
                            todo = todo.OrderBy(query.Sort);
                    }
                    if (query.StartIndex > 0)
                    {
                        todo = todo.Skip(query.StartIndex);
                    }
                    if (query.PageSize > 0)
                    {
                        todo = todo.Take(query.PageSize);
                    }
                }
                result.Items = await todo.ToListAsync();
                result.CurrentCount = result.Items.Count;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        protected async Task<IList<T>> getListByFilterAsync<T>(Expression<Func<T, bool>> filter) where T : class
        {
            IQueryable<T> todo = _absContext.Set<T>();
            if (filter != null)
            {
                todo = todo.Where(filter);
            }
            var items = await todo.ToListAsync();
            return items;
        }
    }
}