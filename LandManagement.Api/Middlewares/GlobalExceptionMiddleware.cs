using System.Net;
using System.Text.Json;
using LandManagement.Api.Exceptions;
using LandManagement.Api.Shared.Models;

namespace LandManagement.Api.Middlewares;

/// <summary>
/// Global exception handling middleware for consistent error responses
/// </summary>
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
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        var apiResponse = new ApiResponse();
        int statusCode;
        string message;

        switch (exception)
        {
            case AppException appEx:
                statusCode = appEx.StatusCode;
                message = appEx.Message;
                if (appEx is ValidationException valEx && valEx.Errors.Any())
                {
                    apiResponse.Errors = valEx.Errors;
                }
                break;

            case KeyNotFoundException:
                statusCode = (int)HttpStatusCode.NotFound;
                message = exception.Message;
                break;

            case UnauthorizedException:
                statusCode = (int)HttpStatusCode.Unauthorized;
                message = exception.Message;
                break;

            case ForbiddenException:
                statusCode = (int)HttpStatusCode.Forbidden;
                message = exception.Message;
                break;

            case ConflictException:
                statusCode = (int)HttpStatusCode.Conflict;
                message = exception.Message;
                break;

            default:
                statusCode = (int)HttpStatusCode.InternalServerError;
                message = "An unexpected error occurred. Please contact support.";
                
                // Log the full exception details internally
                _logger.LogError(exception, "Unhandled exception occurred: {Message}", exception.Message);
                break;
        }

        apiResponse.Success = false;
        apiResponse.Message = message;

        response.StatusCode = statusCode;

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var json = JsonSerializer.Serialize(apiResponse, options);
        await response.WriteAsync(json);
    }
}
