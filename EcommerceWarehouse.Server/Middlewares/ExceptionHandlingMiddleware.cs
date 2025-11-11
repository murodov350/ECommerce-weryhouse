using System.Net;
using System.Text.Json;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace EcommerceWarehouse.Server.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next; _logger = logger;
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

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var status = HttpStatusCode.InternalServerError;
            object payload;

            switch (ex)
            {
                case ValidationException fvEx:
                    status = HttpStatusCode.BadRequest;
                    payload = new { error = "Validation failed", details = fvEx.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }) };
                    break;
                case UnauthorizedAccessException:
                    status = HttpStatusCode.Unauthorized;
                    payload = new { error = "Unauthorized" };
                    break;
                case KeyNotFoundException:
                    status = HttpStatusCode.NotFound;
                    payload = new { error = "Not found" };
                    break;
                case InvalidOperationException:
                    status = HttpStatusCode.BadRequest;
                    payload = new { error = ex.Message };
                    break;
                default:
                    _logger.LogError(ex, "Unhandled exception");
                    payload = new { error = "Server error" };
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;

            var response = new
            {
                statusCode = context.Response.StatusCode,
                traceId = context.TraceIdentifier,
                data = payload
            };

            var json = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }
    }
}
