using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Controllers
{
    [Route("api/[controller]")]
    //[Route("api/{v:apiVersion}/[controller]")]  ----- URL Base
    [ApiVersion("1.0")]  // controller level versioning
    //[Authorize(Policy = "EmployeeOnly")] ------ Policy based Auth
    [Authorize(Policy = "AtLeast18")] //------ Policy based Auth
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private IMemoryCache _memoryCache;

        public ProductsController(IMemoryCache memoryCache)
        {   
            _memoryCache = memoryCache;
        }

        [HttpGet]
        public object Get()
        {
            var cacheKey = "productList";
            object res;
            //checks if cache entries exists
            if (!_memoryCache.TryGetValue(cacheKey, out List<string> productList))
            {
                //calling the server
                productList = new List<string>() { "P1", "P2", "P3" };

                //setting up cache options
                var cacheExpiryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(30),
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = TimeSpan.FromSeconds(20),
                };
                //cacheExpiryOptions.RegisterPostEvictionCallback(PostEvictionCallback, _memoryCache);
                
                
                
                //setting cache entries
                _memoryCache.Set(cacheKey, productList, cacheExpiryOptions); // SET WITH OPTIONS DEFINED ABOVE
                res = (object)productList;
            }
            else
            {
                res = _memoryCache.Get(cacheKey);
            }

            return res;
        }

        [MapToApiVersion("2.0")] // for particular action
        [HttpGet]
        public object GetV2()
        {
            var cacheKey = "productList";
            object res;
            //checks if cache entries exists
            if (!_memoryCache.TryGetValue(cacheKey, out List<string> productList))
            {
                //calling the server
                productList = new List<string>() { "P12", "P22", "P32" };

                //setting up cache options
                var cacheExpiryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddSeconds(30),
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = TimeSpan.FromSeconds(20),
                };
                //cacheExpiryOptions.RegisterPostEvictionCallback(PostEvictionCallback, _memoryCache);



                //setting cache entries
                _memoryCache.Set(cacheKey, productList, cacheExpiryOptions); // SET WITH OPTIONS DEFINED ABOVE
                res = (object)productList;
            }
            else
            {
                res = _memoryCache.Get(cacheKey);
            }

            return res;
        }

        //[NonAction]
        //private static void PostEvictionCallback(object cacheKey, object cacheValue, EvictionReason evictionReason, object state)
        //{
        //    var memoryCache = (IMemoryCache)state;

        //    memoryCache.Set(
        //        "EvictionMessages",
        //        $"Entry {cacheKey} was evicted: {evictionReason}.");
        //}

        [HttpGet("GetWithError")]
        public void GetWithError()
        {
            throw new AccessViolationException("Violation Exception while accessing the resource.");
        }
    }
}
