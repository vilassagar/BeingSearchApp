namespace BeingSearchApp.Server.Middleware
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                _logger.LogInformation($"Request {context.Request.Method} {context.Request.Path} started");

                var startTime = DateTime.UtcNow;
                await _next(context);
                var elapsedMs = (DateTime.UtcNow - startTime).TotalMilliseconds;

                _logger.LogInformation($"Request {context.Request.Method} {context.Request.Path} completed in {elapsedMs}ms with status code {context.Response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Request {context.Request.Method} {context.Request.Path} failed");
                throw;
            }
        }
    }
}
