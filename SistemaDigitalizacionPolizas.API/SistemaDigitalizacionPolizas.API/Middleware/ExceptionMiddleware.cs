using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace SistemaDigitalizacionPolizas.API.Middleware
{
    /// <summary>
    /// Middleware para manejar excepciones globalmente.
    /// Captura errores y devuelve respuestas JSON estandarizadas.
    /// </summary>
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

            // =========================================================
            // 🔴 DUPLICADOS (BASE DE DATOS)
            // =========================================================
            catch (DbUpdateException ex) when (IsDuplicateException(ex))
            {
                _logger.LogWarning(ex, "[Middleware] Duplicado detectado: {Path}", context.Request.Path);

                var campo = ExtractField(ex);
                var valor = ExtractValue(ex);

                var mensaje = valor != null
                    ? $"El {campo} '{valor}' ya existe."
                    : $"Ya existe un registro con ese {campo}.";

                await WriteResponse(context, StatusCodes.Status409Conflict, mensaje);
            }

            // =========================================================
            // 🟠 OTROS ERRORES DE BD
            // =========================================================
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, "[Middleware] Error de base de datos: {Path}", context.Request.Path);

                await WriteResponse(context, StatusCodes.Status422UnprocessableEntity,
                    "No se pudo guardar la información. Verifica los datos.");
            }

            // =========================================================
            // 🟡 REGLAS DE NEGOCIO (TU CASO 🔥)
            // =========================================================
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "[Middleware] Regla de negocio: {Path}", context.Request.Path);

                await WriteResponse(context, StatusCodes.Status409Conflict, ex.Message);
            }

            // =========================================================
            // 🔒 PERMISOS
            // =========================================================
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning(ex, "[Middleware] Acceso no autorizado: {Path}", context.Request.Path);

                await WriteResponse(context, StatusCodes.Status403Forbidden,
                    "No tienes permisos para realizar esta acción.");
            }

            // =========================================================
            // 🔍 NO ENCONTRADO
            // =========================================================
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "[Middleware] Recurso no encontrado: {Path}", context.Request.Path);

                await WriteResponse(context, StatusCodes.Status404NotFound,
                    "El recurso solicitado no existe.");
            }

            // =========================================================
            // ⚠️ VALIDACIÓN
            // =========================================================
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "[Middleware] Argumento inválido: {Path}", context.Request.Path);

                await WriteResponse(context, StatusCodes.Status400BadRequest, ex.Message);
            }

            // =========================================================
            // 💥 ERROR GENERAL
            // =========================================================
            catch (Exception ex)
            {
                _logger.LogError(ex, "[Middleware] Error no controlado en: {Method} {Path}",
                    context.Request.Method, context.Request.Path);

                await WriteResponse(context, StatusCodes.Status500InternalServerError,
                    "Error interno del servidor.");
            }
        }

        // =========================================================
        // 🔥 DETECTAR DUPLICADOS SQL SERVER
        // =========================================================
        private static bool IsDuplicateException(DbUpdateException ex)
        {
            if (ex.InnerException is SqlException sqlEx)
            {
                return sqlEx.Number == 2627 || sqlEx.Number == 2601;
            }

            var message = ex.InnerException?.Message ?? "";

            return message.Contains("duplicate", StringComparison.OrdinalIgnoreCase)
                || message.Contains("UNIQUE", StringComparison.OrdinalIgnoreCase)
                || message.Contains("constraint", StringComparison.OrdinalIgnoreCase)
                || message.Contains("23505");
        }

        // =========================================================
        // 🧠 EXTRAER CAMPO (IX_ o UQ_)
        // =========================================================
        private static string ExtractField(DbUpdateException ex)
        {
            var message = ex.InnerException?.Message ?? "";

            var match = Regex.Match(message, @"'(IX|UQ)_\w+_(\w+)'");

            if (match.Success)
                return match.Groups[2].Value;

            return "dato";
        }

        // =========================================================
        // 🔎 EXTRAER VALOR DUPLICADO
        // =========================================================
        private static string? ExtractValue(DbUpdateException ex)
        {
            var message = ex.InnerException?.Message ?? "";

            var match = Regex.Match(message, @"\((.*?)\)");

            if (match.Success)
                return match.Groups[1].Value;

            return null;
        }

        // =========================================================
        // 📤 RESPUESTA ESTÁNDAR
        // =========================================================
        private static async Task WriteResponse(HttpContext context, int statusCode, string message)
        {
            if (context.Response.HasStarted)
                return;

            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";

            var response = new
            {
                success = false,
                message
            };

            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}