using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NetApp.PlayASPAPI.Controllers
{
    public class ValuesController : ApiController
    {
        public class Apple
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }


        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }

        [Route("apples")]
        public Apple GetApples()
        {
            return new Apple
            {
                Id = Guid.NewGuid().ToString(),
                Name = "asdasd"
            };
        }
    }
}
