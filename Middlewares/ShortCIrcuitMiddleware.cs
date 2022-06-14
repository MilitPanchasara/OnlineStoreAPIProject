using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace OnlineStore.Middlewares
{
    public class ShortCircuitMiddleware
    {
        private readonly RequestDelegate _next;
        public ShortCircuitMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Short circuits pipeline and return response immediately
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
        }

        
    }

    public static class ShortCircuitMiddlewareExtensions
    {
        public static IApplicationBuilder UseShortCircuitMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ShortCircuitMiddleware>();
        }
    }
}