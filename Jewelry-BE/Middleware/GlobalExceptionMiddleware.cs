namespace Jewelry_BE.Middleware
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
                _logger.LogError(ex, "Unexpected exception");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 500;

                var response = new
                {
                    Code = 500,
                    Message = ex.Message,
                    Detailed = ex.StackTrace
                };

                var responseText = System.Text.Json.JsonSerializer.Serialize(response);

                await context.Response.WriteAsync(responseText);
            }
        }
    }
}