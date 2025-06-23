using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;

namespace CoursePlatform.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred.");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = ex switch
                {
                    NotFoundException => (int)HttpStatusCode.NotFound,
                    BadRequestException => (int)HttpStatusCode.BadRequest,
                    ArgumentNullException => (int)HttpStatusCode.BadRequest,
                    ArgumentException => (int)HttpStatusCode.BadRequest,
                    KeyNotFoundException => (int)HttpStatusCode.NotFound,
                    UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                    _ => (int)HttpStatusCode.InternalServerError
                };

                var result = JsonSerializer.Serialize(new
                {
                    error = ex.Message,
                    status = context.Response.StatusCode
                });

                await context.Response.WriteAsync(result);
            }
        }
    }
}