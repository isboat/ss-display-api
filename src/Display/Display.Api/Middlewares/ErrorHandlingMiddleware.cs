using System;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Display.Shared.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Display.Api.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly ILogger _logger;
        private readonly RequestDelegate _next;
        private readonly JsonSerializerOptions _jsonSerializerOptions;

        public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger, RequestDelegate next)
        {
            _logger = logger as ILogger ?? NullLogger.Instance;
            _next = next;
            _jsonSerializerOptions = new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
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

        private Task HandleExceptionAsync(HttpContext context, Exception exception) => exception switch
        {
            AccessForbiddenException ex => HandleExpectedAsync(context, ex, StatusCodes.Status403Forbidden, "access_denied", "Forbidden"),
            InvalidDevicecodeException ex => HandleExpectedAsync(context, ex, StatusCodes.Status403Forbidden, "access_denied", "Invalid device code"),
            InvalidTenantException ex => HandleExpectedAsync(context, ex, StatusCodes.Status404NotFound, "access_denied", "Invalid Tenant"),
            AccessExpiredException ex => HandleExpectedAsync(context, ex, StatusCodes.Status400BadRequest, "access_expired", "Access Expired"),
            InvalidRefreshTokenException ex => HandleExpectedAsync(context, ex,StatusCodes.Status403Forbidden, "refresh_token_invalid", "Refresh Token Invalid"),
            AuthorizationPendingException ex => HandleExpectedAsync(context, ex, StatusCodes.Status428PreconditionRequired, "authorization_pending", "Precondition Required"),
            _ => HandleUnexpectedAsync(context, exception)
        };

        private async Task HandleExpectedAsync(HttpContext context, Exception exception, int statusCode, string code, string message)
        {
            //LogError(context, exception);
            await HandleExceptionAsync(context, exception, statusCode, code, message ?? "Revisit your arguments and try again");
        }

        private async Task HandleUnexpectedAsync(HttpContext context, Exception exception)
        {
            //LogError(context, exception, "Unexpected exception");
            await HandleExceptionAsync(context, exception, StatusCodes.Status500InternalServerError, "ierr-0001", null);
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex, int statusCode, string errorCode, string? message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                error = errorCode ?? ex.GetType().Name,
                error_description = message ?? ex.Message,
            }, _jsonSerializerOptions));
        }

        private void LogError(HttpContext context, Exception exception, string? message = null) =>
          _logger.LogError(exception, "{Message}; request='{Method} {Path}', message={Exception}",
            message ?? exception.Message, context.Request.Method, context.Request.Path.Value, exception.Message);
    }
}