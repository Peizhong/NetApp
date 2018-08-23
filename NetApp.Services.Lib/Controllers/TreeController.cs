using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;
using NetApp.Models.Interfaces;
using NetApp.Repository.Interfaces;

namespace NetApp.Services.Lib.Controllers
{
    public abstract class TreeController<T> : ListController<T> where T : ITreeNode<T>
    {
        private readonly ITreeRepo<T> _repo;

        public TreeController(ILogger<TreeController<T>> logger, IDistributedCache cache, ITreeRepo<T> repo) :
            base(logger, cache, repo)
        {
            _repo = repo;
        }

        [HttpGet("{id}/children")]
        public async Task<IList<T>> Children(string id)
        {
            Func<Task<IList<T>>> query = async () =>
            {
                return await _repo.GetListAsync(t => t.ParentId == id, null);
            };
            var result = await CacheIt(query);
            return result;
        }
    }
}