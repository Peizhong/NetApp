using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Distributed;
using NetApp.Models.Interfaces;
using NetApp.Repository.Interfaces;

namespace NetApp.Services.Lib.Controllers
{
    public abstract class TreeController<T> : ListController<T> where T : ITreeNode<T>
    {
        public TreeController(ILogger<TreeController<T>> logger, IDistributedCache cache, ITreeRepo<T> repo) :
            base(logger, cache, repo)
        {

        }

        [HttpGet("{id}/children")]
        public IEnumerable<T> Children(string id)
        {
            return null;
        }
    }
}