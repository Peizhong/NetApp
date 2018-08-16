using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetApp.Entities.Interfaces;
using NetApp.Entities.Mall;

namespace NetApp.Service.Controllers
{
    public abstract class TreeController<T> : SimpleController<T> where T : ITreeNode, new()
    {
        public TreeController()
        {
            foreach(var r in Repo)
            {
                r.ParentId = "3";
            }
        }

        [HttpGet("{id}/children")]
        public IEnumerable<T> Children(string id)
        {
            var current = Repo.Where(t => t.ParentId == id);
            return current;
        }
    }

    public class Product2Controller : SimpleController<Product2>
    {

    }

    public class Category2Controller : TreeController<Category2>
    {

    }
}