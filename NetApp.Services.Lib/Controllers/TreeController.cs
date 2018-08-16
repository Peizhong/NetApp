using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using NetApp.Entities.Interfaces;

namespace NetApp.Services.Lib.Controllers
{
    public abstract class TreeController<T> : ListController<T> where T : ITreeNode
    {
        [HttpGet("{id}/children")]
        public IEnumerable<T> Children(string id)
        {
            return null;
        }
    }
}