using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;
using NetApp.Common.Abstractions;
using AutoMapper;

namespace NetApp.Services.Lib.Controllers
{
    public abstract class TreeController<T> : ListController<T> where T : ITreeNode<T>
    {
        public class LiteTreeNode
        {
            public string Id { get; set; }

            public string Name { get; set; }

            //public string FullPath { get; set; }

            public virtual List<LiteTreeNode> Children { get; set; }
        }

        static IMapper _mapper = null;

        static TreeController()
        {
            if (_mapper == null)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<T, LiteTreeNode>();

                    cfg.CreateMap<PageableQueryResult<T>, PageableQueryResult<LiteTreeNode>>();
                });
                _mapper = config.CreateMapper();

                config.AssertConfigurationIsValid();
            }
        }

        private readonly new ITreeRepo<T> _repo;

        public TreeController(ILogger<TreeController<T>> logger, IDistributedCache cache, ITreeRepo<T> repo) :
            base(logger, cache, repo)
        {
            _repo = repo;
        }

        [HttpGet("{id}/children")]
        public async Task<PageableQueryResult<T>> Children(string id)
        {
            var page = new PageableQuery<T>();
            Func<Task<PageableQueryResult<T>>> query = async () =>
            {
                page.Filter = t => t.DataStatus != 2 && t.ParentId == id;
                page.Sort = t => t.SortNo;
                var gen1 = await _repo.GetListAsync(page);
                if (gen1.CurrentCount > 0)
                {
                    var gen1Ids = gen1.Items.Select(p => p.Id).ToList();
                    page.Filter = t => t.DataStatus != 2 && gen1Ids.Contains(t.ParentId);
                    var gen2 = await _repo.GetListAsync(page);
                }
                return gen1;
            };
            var result = await CacheIt(query);
            result.Host = $"{Request.Host}";
            return result;
        }

        [HttpGet("{id}/children/lite")]
        public async Task<PageableQueryResult<LiteTreeNode>> ChildrenLite(string id)
        {
            var fullResult = await Children(id);
            var liteResult = _mapper.Map<PageableQueryResult<LiteTreeNode>>(fullResult);
            return liteResult;
        }
    }
}