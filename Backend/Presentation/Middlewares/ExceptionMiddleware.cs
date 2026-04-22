using System.Data;
using System.Net;
using System.Text.Json;
using Application.Response;
using Domain.Exceptions;

namespace Presentation.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
                _logger.LogError(ex, "Ocurrió un error no controlado");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            // Mapeamos tipos de excepciones a códigos HTTP
            var statusCode = exception switch
            {
                BadRequestException => (int)HttpStatusCode.BadRequest, // Errores de lógica, 400
                KeyNotFoundException => (int)HttpStatusCode.NotFound,   // No encontrado, 404
                DuplicateNameException => (int)HttpStatusCode.Conflict, // 409
                _ => (int)HttpStatusCode.InternalServerError            // Error 500
            };

            context.Response.StatusCode = statusCode;

            var response = new ApiErrorResponse
            {
                StatusCode = statusCode,
                Message = exception.Message 
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response, JsonOptions));
        }
    }
}
