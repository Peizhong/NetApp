using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetApp.PlayWeb.Gateway
{
    public class PipelineContext
    {
        public HttpContext HttpContext { get; }

        public PipelineContext(HttpContext context)
        {
            HttpContext = context;
        }
    }
}
