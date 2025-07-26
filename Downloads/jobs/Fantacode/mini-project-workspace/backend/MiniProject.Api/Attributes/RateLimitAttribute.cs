using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace MiniProject.Api.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RateLimitAttribute : ActionFilterAttribute
    {
        private readonly IMemoryCache _cache;
        private readonly int _seconds;
        private readonly int _maxRequests;

        public RateLimitAttribute(int seconds = 60, int maxRequests = 5)
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
            _seconds = seconds;
            _maxRequests = maxRequests;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var ipAddress = context.HttpContext.Connection.RemoteIpAddress?.ToString();
            var cacheKey = $"RateLimit_{ipAddress}";

            if (!_cache.TryGetValue(cacheKey, out int requests))
            {
                requests = 1;
                _cache.Set(cacheKey, requests, TimeSpan.FromSeconds(_seconds));
            }
            else
            {
                requests++;
                if (requests > _maxRequests)
                {
                    context.Result = new StatusCodeResult(429); // Too Many Requests
                    return;
                }
                _cache.Set(cacheKey, requests, TimeSpan.FromSeconds(_seconds));
            }

            base.OnActionExecuting(context);
        }
    }
}
