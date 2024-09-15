using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net;
using System;
using System.Threading.Tasks;
using PromoCodeFactory.Core.Domain.Exceptions;

namespace PromoCodeFactory.WebHost.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class NotFoundExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public NotFoundExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (NotFoundException ex)
            {
                await HandleExceptionMessageAsync(context, ex).ConfigureAwait(false);
            }
        }

        private static Task HandleExceptionMessageAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            int statusCode = (int)HttpStatusCode.NotFound;
            var result = JsonConvert.SerializeObject(new
            {
                StatusCode = statusCode,
                ErrorMessage = exception.Message
            });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsync(result);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class NotFoundExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseNotFoundExceptionHandlingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<NotFoundExceptionHandlingMiddleware>();
        }
    }
}
