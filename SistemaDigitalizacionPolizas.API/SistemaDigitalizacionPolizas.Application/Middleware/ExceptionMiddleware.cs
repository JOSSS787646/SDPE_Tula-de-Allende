using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;



    namespace SistemaDigitalizacionPolizas.API.Middleware
    {
        public class ExceptionMiddleware
        {

        //Permite lanzar los errores de UnauthorizedAccessException como 403 Forbidden
        private readonly RequestDelegate _next;

            public ExceptionMiddleware(RequestDelegate next)
            {
                _next = next;
            }

            public async Task Invoke(HttpContext context)
            {
                try
                {
                    await _next(context);
                }
                catch (UnauthorizedAccessException ex)
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    context.Response.ContentType = "application/json";

                    await context.Response.WriteAsync(JsonSerializer.Serialize(new
                    {
                        success = false,
                        message = ex.Message
                    }));
                }
                catch (Exception)
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    context.Response.ContentType = "application/json";

                    await context.Response.WriteAsync(JsonSerializer.Serialize(new
                    {
                        success = false,
                        message = "Error interno del servidor"
                    }));
                }
            }
        }
    }


