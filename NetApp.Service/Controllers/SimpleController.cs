using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NetApp.Entities.Interfaces;
using NetApp.Service.Models;

namespace NetApp.Service.Controllers
{
    [Produces("application/json")]
    [ApiController]
    [Route("api/[controller]")]
    public abstract class SimpleController<T> : ControllerBase where T : IQuery, new()
    {
        protected IEnumerable<T> Repo { get; }

        public SimpleController()
        {
            Repo = Enumerable.Range(1, 10).Select(n =>
            {
                var t = new T();
                t.Id = n.ToString();
                return t;
            }).ToList();
        }

        // GET: api/Simple
        [HttpGet]
        public IEnumerable<T> Get([FromQuery]Filterable<T> filter)
        {
            return Repo.Skip(filter.StartIndex).Take(filter.PageSize);
        }

        // GET: api/Simple/5
        [HttpGet("{id}")]
        public T Get(string id)
        {
            return Repo.FirstOrDefault(r => r.Id == id);
        }

        // POST: api/Simple
        [HttpPost]
        public void Post([FromBody] T value)
        {
        }

        // PUT: api/Simple/5
        [HttpPut("{id}")]
        public void Put(string id, [FromBody] T value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
        }
    }
}