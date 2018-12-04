using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetApp.PlayWeb.Gateway
{
    public abstract class PipelineMiddleware
    {
        public abstract Task Invoke(PipelineContext context);
    }

    public class ExceptionMiddleware : PipelineMiddleware
    {
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly PipelineDelegate _next;

        public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger, PipelineDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public override async Task Invoke(PipelineContext context)
        {
            _logger.LogInformation("hello ExceptionMiddleware");
            try
            {
                await _next?.Invoke(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }

    public class CacheMiddleware : PipelineMiddleware
    {
        private readonly ILogger<CacheMiddleware> _logger;
        private readonly PipelineDelegate _next;

        public bool IsCached { get; set; }

        public CacheMiddleware(ILogger<CacheMiddleware> logger, PipelineDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public override async Task Invoke(PipelineContext context)
        {
            _logger.LogInformation("hello CacheMiddleware");
            if (IsCached)
            {
                //get something from cache
                ;
                return;
            }
            await _next?.Invoke(context);
            //set something to cache
            ;
        }
    }

    public class ReRouteMiddleware : PipelineMiddleware
    {
        private readonly ILogger<ReRouteMiddleware> _logger;
        private readonly GatewayOption _options;
        private readonly PipelineDelegate _next;

        public ReRouteMiddleware(ILogger<ReRouteMiddleware> logger, IOptions<GatewayOption> options, PipelineDelegate next)
        {
            _logger = logger;
            _options = options.Value;
            _next = next;
        }

        public override async Task Invoke(PipelineContext context)
        {
            _logger.LogInformation("hello ReRouteMiddleware");
            _logger.LogInformation("config from " + _options.Author);

            string requestPath = context.HttpContext.Request.Path;
            foreach(var route in _options.ReRoutes)
            {
                if (route.UpstreamPathTemplate.StartsWith(requestPath))
                {
                    _logger.LogInformation("match url");
                }
            }

            await _next?.Invoke(context);
        }
    }
    
    /// <summary>
    /// 获得downstream返回数据
    /// </summary>
    public class RequestMiddleware : PipelineMiddleware
    {
        private readonly ILogger<RequestMiddleware> _logger;
        private readonly PipelineDelegate _next;

        public RequestMiddleware(ILogger<RequestMiddleware> logger, PipelineDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public override async Task Invoke(PipelineContext context)
        {
            _logger.LogInformation("hello RequestMiddleware");
            await _next?.Invoke(context);
        }
    }

    /// <summary>
    /// 返回给upstream调用方
    /// </summary>
    public class ResponseMiddleware : PipelineMiddleware
    {
        private readonly ILogger<ResponseMiddleware> _logger;
        private readonly PipelineDelegate _next;

        public ResponseMiddleware(ILogger<ResponseMiddleware> logger, PipelineDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public override async Task Invoke(PipelineContext context)
        {
            _logger.LogInformation("hello ResponseMiddleware");
            await _next?.Invoke(context);
        }
    }
}
