using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;

namespace NetApp.Middlewares
{
    public class RequestInfoMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestInfoMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// middleware Task method's name can be either Invoke or InvokeAsync
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public Task InvokeAsync(HttpContext httpContext)
        {
            Console.WriteLine($@"RequestSetOptionsMiddleware.Invoke for {httpContext.Request.Path}");

            var str = httpContext.Session.GetString("_Name");

            // Call the next delegate/middleware in the pipeline
            return this._next(httpContext);
        }
    }

    //exposes the middleware through IApplicationBuilder:
    public static class RequestInfoMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestInfo(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestInfoMiddleware>();
        }
    }
}