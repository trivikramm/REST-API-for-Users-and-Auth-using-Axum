using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace LoginDashboardAPI.Middleware
{
    public class RateLimitMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMemoryCache _cache;
        private const int MaxRequests = 5; // Max requests allowed
        private static readonly TimeSpan TimeWindow = TimeSpan.FromMinutes(1); // Time window for rate limiting

        public RateLimitMiddleware(RequestDelegate next, IMemoryCache cache)
        {
            _next = next;
            _cache = cache;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.Request.Path.StartsWithSegments("/api/auth/login"))
            {
                var ipAddress = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                var cacheKey = $"rate_limit_{ipAddress}";
                
                // Get current request count for this IP
                _cache.TryGetValue(cacheKey, out int requestCount);
                
                if (requestCount >= MaxRequests)
                {
                    context.Response.StatusCode = StatusCodes.Status429TooManyRequests;
                    context.Response.Headers["Retry-After"] = TimeWindow.TotalSeconds.ToString();
                    await context.Response.WriteAsync("Too many requests. Please try again later.");
                    return;
                }
                
                // Increment the request count and set cache
                _cache.Set(cacheKey, requestCount + 1, TimeWindow);
            }

            await _next(context);
        }
    }
}