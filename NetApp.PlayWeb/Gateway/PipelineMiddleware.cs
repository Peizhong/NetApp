using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
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

        private Dictionary<string, string> _cache = new Dictionary<string, string>();

        public CacheMiddleware(
            ILogger<CacheMiddleware> logger,
            PipelineDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public override async Task Invoke(PipelineContext context)
        {
            _logger.LogInformation("hello CacheMiddleware");
            if (context.RouteConfig.CacheTtlSeconds>0)
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

        public ReRouteMiddleware(
            ILogger<ReRouteMiddleware> logger, 
            IOptions<GatewayOption> options, 
            PipelineDelegate next)
        {
            _logger = logger;
            _options = options.Value;
            _next = next;
        }

        public override async Task Invoke(PipelineContext context)
        {
            _logger.LogInformation("hello ReRouteMiddleware");
            _logger.LogInformation("config from " + _options.Author);

            string requestPath = context.UpstreamHttpContext.Request.Path;
            foreach (var route in _options.Routes)
            {
                // 暂时不做复杂转换，只改开头，不比较大小写
                if (requestPath.ToLower().StartsWith(route.UpstreamPathTemplate.ToLower()))
                {
                    if (route.UpstreamHttpMethod.Count == 0 ||
                        route.UpstreamHttpMethod.Contains(context.UpstreamHttpContext.Request.Method))
                    {
                        string downstreamPath = requestPath.Replace(route.UpstreamPathTemplate, route.DownstreamPathTemplate);
                        _logger.LogInformation($"match method: {context.UpstreamHttpContext.Request.Method}, url: from {requestPath} to {downstreamPath}");

                        context.DownstreamPath = downstreamPath;
                        context.DownstreamQueryString = context.UpstreamHttpContext.Request.QueryString.Value;
                        context.RouteConfig = route;
                        break;
                    }
                }
            }
            _logger.LogInformation("reroute complete");
            await _next?.Invoke(context);
        }
    }

    /// <summary>
    /// 获得downstream返回数据
    /// </summary>
    public class RequestMiddleware : PipelineMiddleware
    {
        private readonly ILogger<RequestMiddleware> _logger;
        private readonly IDownstreamSelector _downstreamSelector;
        private readonly PipelineDelegate _next;

        private readonly string[] _unsupportedRequestHeaders =
        {
            "Cookie",
            "Transfer-Encoding"
        };

        public RequestMiddleware(
            ILogger<RequestMiddleware> logger,
            IDownstreamSelector downstreamSelector,
            PipelineDelegate next)
        {
            _logger = logger;
            _downstreamSelector = downstreamSelector;
            _next = next;
        }

        public override async Task Invoke(PipelineContext context)
        {
            _logger.LogInformation("hello RequestMiddleware");

            var downstream = _downstreamSelector.GetHostAndPort(context.RouteConfig.ServiceName, context.RouteConfig.DownstreamHostAndPorts);
            context.DownstreamHost = downstream.Host;
            context.DownstreamPort = downstream.Port;

            using (HttpClient client = new HttpClient())
            {
                var request = new HttpRequestMessage
                {
                    Method = new HttpMethod(context.UpstreamHttpContext.Request.Method),
                    RequestUri = new Uri($"http://{context.DownstreamHost}:{context.DownstreamPort}{context.DownstreamPath}{context.DownstreamQueryString}"),
                };
                foreach (var head in context.UpstreamHttpContext.Request.Headers)
                {
                    if (_unsupportedRequestHeaders.Contains(head.Key))
                    {
                        _logger.LogInformation($"remove header '{head.Key}' in request.headers");
                        continue;
                    }
                    request.Headers.Add(head.Key, head.Value.ToArray());
                    _logger.LogInformation($"set header '{head.Key}' in request.headers");
                }
                //todo: custom cookies?
                var cookies = context.UpstreamHttpContext.Request.Cookies.Select(c => $"{c.Key}={c.Value}");
                cookies = cookies.Append("gateway=test");
                var cookieStr = string.Join(';', cookies);
                if (!string.IsNullOrEmpty(cookieStr))
                {
                    request.Headers.Add("Cookie", cookieStr);
                }
                _logger.LogInformation($"request url is: {request.RequestUri}");
                var response = await client.SendAsync(request);
                context.DownstreamResponse = response;
            }
            _logger.LogInformation("request complete");
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

        private readonly string[] _unsupportedRequestHeaders =
        {
            "Transfer-Encoding"
        };

        public ResponseMiddleware(ILogger<ResponseMiddleware> logger, PipelineDelegate next)
        {
            _logger = logger;
            _next = next;
        }

        public override async Task Invoke(PipelineContext context)
        {
            _logger.LogInformation("hello ResponseMiddleware");

            //添加downstream返回的文件头
            foreach (var head in context.DownstreamResponse.Headers)
            {
                if (_unsupportedRequestHeaders.Contains(head.Key))
                {
                    _logger.LogInformation($"remove header '{head.Key}' in respones.headers");
                    continue;
                }
                context.UpstreamHttpContext.Response.Headers.TryAdd(head.Key, head.Value.FirstOrDefault());
                _logger.LogInformation($"set header '{head.Key}' in respones.headers");
            }
            foreach (var head in context.DownstreamResponse.Content.Headers)
            {
                if (_unsupportedRequestHeaders.Contains(head.Key))
                {
                    _logger.LogInformation($"remove header '{head.Key}' in respones.content.headers");
                    continue;
                }
                context.UpstreamHttpContext.Response.Headers.TryAdd(head.Key, head.Value.FirstOrDefault());
                _logger.LogInformation($"set header '{head.Key}' in respones.content.headers");
            }
            //拷贝content
            using (var content = await context.DownstreamResponse.Content.ReadAsStreamAsync())
            {
                context.UpstreamHttpContext.Response.ContentLength = context.DownstreamResponse.Content.Headers.ContentLength;

                context.UpstreamHttpContext.Response.StatusCode = (int)context.DownstreamResponse.StatusCode;

                context.UpstreamHttpContext.Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = context.DownstreamResponse.ReasonPhrase;

                if (context.UpstreamHttpContext.Response.StatusCode != (int)HttpStatusCode.NotModified && context.UpstreamHttpContext.Response.ContentLength != 0)
                {
                    await content.CopyToAsync(context.UpstreamHttpContext.Response.Body);
                }
            }

            _logger.LogInformation("response complete");
            await _next?.Invoke(context);
        }
    }
}
