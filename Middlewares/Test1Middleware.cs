using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Middlewares
{
    public class Test1Middleware
    {
        RequestDelegate next;
        public Test1Middleware(RequestDelegate _next)
        {
            next = _next;
        }
        public async Task Invoke(HttpContext context)
        {
            await next(context);
        }
    }
}
