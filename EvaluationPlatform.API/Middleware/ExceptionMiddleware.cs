using CheckMate.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace CheckMate.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger)
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
                _logger.LogError(
                    ex,
                    "Unhandled exception. Method: {Method}, Path: {Path}",
                    context.Request.Method,
                    context.Request.Path);

                if (ex is ConflictException)
                {
                    context.Response.StatusCode =
                        StatusCodes.Status409Conflict;

                    context.Response.ContentType =
                        "application/json";

                    await context.Response.WriteAsJsonAsync(new
                    {
                        success = false,
                        message = ex.Message
                    });

                    return;
                }

                if (ex is UnauthorizedException)
                {
                    context.Response.StatusCode =
                        StatusCodes.Status401Unauthorized;

                    context.Response.ContentType =
                        "application/json";

                    await context.Response.WriteAsJsonAsync(new
                    {
                        success = false,
                        message = ex.Message
                    });

                    return;
                }
                if (ex is NotFoundException)
                {
                    context.Response.StatusCode =
                        StatusCodes.Status404NotFound;

                    context.Response.ContentType =
                        "application/json";

                    await context.Response.WriteAsJsonAsync(new
                    {
                        success = false,
                        message = ex.Message
                    });

                    return;
                }

                await HandleExceptionAsync(context, ex, _logger);
            }           
        }

        private static async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception,
            ILogger<ExceptionMiddleware> logger)
        {
            context.Response.StatusCode =
                (int)HttpStatusCode.InternalServerError;

            context.Response.ContentType = "application/json";

            var response = new
            {
                success = false,
                message = "An unexpected error occurred."
            }; 

            logger.LogError("Sending error response: {@Response}", response);

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response));
        }
    }
}
