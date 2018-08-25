﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
using NetApp.Models.Interfaces;

namespace NetApp.Repository.Interfaces
{
    public interface IListRepo<T> where T : IBase
    {
        Task<T> FindAsync(string id);

        Task<T> GetAsync(IQuery<T> query);

        Task<PageableQueryResult<T>> GetListAsync(PageableQuery<T> query);

        Task UpdateAsync(T value);

        Task UpdateListAsync(IEnumerable<T> values);
    }
}