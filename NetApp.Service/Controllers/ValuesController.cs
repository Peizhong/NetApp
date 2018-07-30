using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetApp.Service.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace NetApp.Service.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly MallContext _context;

        public ValuesController(MallContext context)
        {
            _context = context;
        }

        [HttpGet("[action]/{id}")]
        public async Task<ActionResult<string>> MallUser(string id)
        {
            var user = await _context.Users.OrderBy(u => u.UserName).Skip(100).FirstOrDefaultAsync();
            var bills = await _context.Orders.Where(o => o.User == user).ToArrayAsync();
            //var detail = await _context.OrderDetails.Include(d => d.Product).Include(d => d.Order).ThenInclude(o => o.User).Where(o => o.Order.User == user).ToArrayAsync();
            return JsonConvert.SerializeObject(user, settings: new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            var host = HttpContext.Request.Host.Value;
            return new string[] { "value1", "value2", "from", host };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
