using System.Text.Json;

namespace Api.Middlewares
{
    public class ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger = logger;

        public async Task Invoke(HttpContext context)
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
            var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            context.Response.ContentType = "application/json";

            if (!context.Response.HasStarted)
                context.Response.StatusCode = 500;
            object errors = new { code = ex.Message, description = ex.StackTrace ?? null };
            object response = new { message = "Ha ocurrido un error en el servidor", isSuccess = false, operationHandled = false, errors, jsonOptions };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response, jsonOptions));
            LogError(context, errors);
        }


        private void LogError(HttpContext context, object errors)
        {
            _logger.LogError("Ocurrió un error mientras se ejecutaba la petición: {Method} {Path}", context.Request.Method, context.Request.Path);
            _logger.LogError("Error details: {Errors}", errors.ToString());
        }
    }
}
