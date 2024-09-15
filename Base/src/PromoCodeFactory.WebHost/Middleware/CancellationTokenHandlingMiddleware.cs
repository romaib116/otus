using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace PromoCodeFactory.WebHost.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class CancellationTokenHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public CancellationTokenHandlingMiddleware(RequestDelegate next, ILogger<CancellationTokenHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception _) when (_ is OperationCanceledException or TaskCanceledException)
            {
                _logger.LogInformation("Shit happened");
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class CancellationTokenHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseCancellationTokenHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CancellationTokenHandlingMiddleware>();
        }
    }
}
