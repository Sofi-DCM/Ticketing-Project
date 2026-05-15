
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
                // --- 400 Bad Request ---
                ApplicationException => (int)HttpStatusCode.BadRequest,
                ArgumentException => (int)HttpStatusCode.BadRequest,
                BadRequestException =>(int)HttpStatusCode.BadRequest,
                // --- 401 Unauthorized ---
                UnauthorizedException => (int)HttpStatusCode.Unauthorized,
                // --- 404 Not Found ---
                KeyNotFoundException => (int)HttpStatusCode.NotFound,   
                NotFoundException => (int)HttpStatusCode.NotFound,
                // --- 409 Conflict ---
                DuplicateNameException => (int)HttpStatusCode.Conflict,
                InvalidOperationException => (int)HttpStatusCode.Conflict, 
                DbUpdateConcurrencyException => (int)HttpStatusCode.Conflict,
                ConflictException => (int)HttpStatusCode.Conflict,
                // --- 500 Server Error ---
                _ => (int)HttpStatusCode.InternalServerError         
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
