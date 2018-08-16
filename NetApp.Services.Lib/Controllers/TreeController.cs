using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetApp.Entities.Interfaces;
using NetApp.Repository.Interfaces;

namespace NetApp.Services.Lib.Controllers
{
    public abstract class TreeController<T> : ListController<T> where T : ITreeNode
    {
        public TreeController(ILogger<TreeController<T>> logger, ITreeRepo<T> repo) :
            base(logger, repo)
        {

        }

        [HttpGet("{id}/children")]
        public IEnumerable<T> Children(string id)
        {
            return null;
        }
    }
}