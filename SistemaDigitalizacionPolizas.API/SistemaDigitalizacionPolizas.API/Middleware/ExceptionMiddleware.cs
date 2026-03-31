using System.Text.Json;

namespace SistemaDigitalizacionPolizas.API.Middleware
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

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "[Middleware] Acceso no autorizado: {Path}", context.Request.Path);
                await WriteResponse(context, StatusCodes.Status403Forbidden, "No tienes permisos para realizar esta acción.");
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "[Middleware] Recurso no encontrado: {Path}", context.Request.Path);
                await WriteResponse(context, StatusCodes.Status404NotFound, "El recurso solicitado no existe.");
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "[Middleware] Argumento inválido: {Path}", context.Request.Path);
                await WriteResponse(context, StatusCodes.Status400BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                // ✅ TÚ ves el error completo en los logs
                // ❌ El cliente solo ve "Error interno del servidor"
                _logger.LogError(ex, "[Middleware] Error no controlado en: {Method} {Path}",
                    context.Request.Method, context.Request.Path);

                await WriteResponse(context, StatusCodes.Status500InternalServerError, "Error interno del servidor.");
            }
        }

        private static async Task WriteResponse(HttpContext context, int statusCode, string message)
        {
            if (context.Response.HasStarted)
                return;

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsync(JsonSerializer.Serialize(new
            {
                success = false,
                message
            }));
        }
    }
}