using FinanceDashboard.Commons.Utilities;
using StatusCodes = FinanceDashboard.Commons.Utilities.StatusCodes;

namespace FinanceDashboard.Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

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
                _logger.LogError(ex, ex.Message);

                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            int statusCode = StatusCodes.InternalServerError;
            string message = ResponseMessages.ServerError;

            if (ex is UnauthorizedAccessException)
            {
                statusCode = StatusCodes.Unauthorized;
                message = ResponseMessages.Unauthorized;
            }
            else if (ex is KeyNotFoundException)
            {
                statusCode = StatusCodes.NotFound;
                message = ResponseMessages.RecordNotFound;
            }

            var response = new Response<string>
            {
                StatusCode = statusCode,
                Message = message,
                Errors = new List<string> { ex.Message }
            };

            context.Response.StatusCode = statusCode;
            return context.Response.WriteAsJsonAsync(response);
        }
    }
}
