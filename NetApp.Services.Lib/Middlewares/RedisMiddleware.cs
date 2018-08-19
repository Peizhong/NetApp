using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using StackExchange.Redis;

namespace NetApp.Services.Lib.Middlewares
{
    public class RedisMiddleware : IMiddleware
    {
        private readonly ConnectionMultiplexer _redis;

        public RedisMiddleware(string connectionString)
        {
            _redis = ConnectionMultiplexer.Connect(connectionString);
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            /*
            IDatabase db = _redis.GetDatabase();
            string path = context.Request.Path;
            var value = await db.StringGetAsync(path);
            if (!value.IsNullOrEmpty)
            {
                await context.Response.WriteAsync(value);
                return;
            }
            await next(context);
            string responseBody = new StreamReader(context.Response.Body).ReadToEnd();*/
            await next(context);
        }
    }

    public static class RedisMiddlewareExtension
    {
        public static IApplicationBuilder UserRedisCacheResponse(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RedisMiddleware>();
        }
    }
}
