using Application.Commom;

namespace API.MiddleWares
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
            var originalBodyStream = context.Response.Body;
            var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            try
            {
                // Log de entrada
                _logger.LogInformation("HTTP {method} {url} from {ip}",
                    context.Request.Method,
                    context.Request.Path,
                    context.Connection.RemoteIpAddress?.ToString());

                await _next(context);

                // Leer y loguear respuesta
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
                context.Response.Body.Seek(0, SeekOrigin.Begin);

                _logger.LogInformation("HTTP {statusCode} Response: {response}",
                    context.Response.StatusCode,
                    responseText);

                await responseBody.CopyToAsync(originalBodyStream);
            }
            catch (APIExceptions ex)
            {
                context.Response.Body = originalBodyStream;
                _logger.LogWarning("API Exception - {Message} - Code: {ErrorCode}", ex.Message, ex.ErrorCode);
                await HandleExceptionAsync(context, ex.StatusCode, ex.Message, ex.ErrorCode);
            }
            catch (Exception ex)
            {
                context.Response.Body = originalBodyStream;
                _logger.LogError(ex, "Unhandled Exception - {Message}", ex.Message);
                await HandleExceptionAsync(context, 500, "Ha ocurrido un error inesperado.");
            }
            finally
            {
                responseBody.Dispose();
            }
        }

        private Task HandleExceptionAsync(HttpContext context, int statusCode, string message, string? errorCode = null)
        {
            context.Response.Clear();
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var response = new
            {
                status = statusCode,
                message,
                errorCode
            };

            return context.Response.WriteAsJsonAsync(response);
        }
    }

}

