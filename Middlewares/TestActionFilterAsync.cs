using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Middlewares
{
    public class TestActionFilterAsync : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // LOGIC TO RUN BEFORE ACTION 
            var query = context.HttpContext.Request.QueryString.Value;

            await next();

            // LOGIC TO RUN AFTER ACTION 
            var responseStatusCode = context.HttpContext.Response.StatusCode;
        }
    }
}
