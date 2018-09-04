using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetApp.Services.Lib.Extensions
{
    public static class IdentityServerAuthenticationExtensions
    {
        public static void AddMyIdentityServerAuthentication(this IServiceCollection services, string identityServer, string apiName)
        {
            services
                .AddAuthentication("Bearer")
                .AddIdentityServerAuthentication(options =>
                {
                    options.Authority = identityServer;
                    options.RequireHttpsMetadata = false;
                    options.ApiName = apiName;
                });
        }
    }
}