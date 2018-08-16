using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetApp.Entities.Interfaces;
using NetApp.Repository.Interfaces;

namespace NetApp.Services.Lib.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class ListController<T> : ControllerBase where T : IQuery
    {
        protected readonly ILogger<ListController<T>> _logger;
        protected readonly IListRepo<T> _repo;

        public ListController(ILogger<ListController<T>> logger, IListRepo<T> repo)
        {
            _logger = logger;
            _repo = repo;
        }

        // GET: api/Products
        [HttpGet]
        public Task<IList<T>> GetAsync()
        {
            return _repo.GetListAsync(null);
        }

        // GET: api/Products/5
        [HttpGet("{id}", Name = "Get")]
        public Task<T> GetAsync(string id)
        {
            return _repo.FindAsync(id);
        }

        // POST: api/Products
        [HttpPost]
        public Task PostAsync([FromBody] T value)
        {
            return _repo.UpdateAsync(value);
        }

        // PUT: api/Products/5
        [HttpPut("{id}")]
        public Task PutAsync(int id, [FromBody] T value)
        {
            return _repo.UpdateAsync(value);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}