using CoursePlatform.Application.Common.Exceptions;
using System.Net;
using System.Text.Json;

namespace CoursePlatform.Application.Common.Middleware;

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
            _logger.LogError(ex, "An unexpected error occurred");
            await HandleExceptionAsync(context, ex);
        }
    }    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = context.Response;
        object errorResponse;

        switch (exception)
        {
            case NotFoundException:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                errorResponse = new { message = exception.Message };
                break;
            case BadRequestException:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResponse = new { message = exception.Message };
                break;
            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                errorResponse = new { message = "An internal server error occurred." };
                break;
        }

        var jsonResponse = JsonSerializer.Serialize(errorResponse);
        await context.Response.WriteAsync(jsonResponse);
    }
}
