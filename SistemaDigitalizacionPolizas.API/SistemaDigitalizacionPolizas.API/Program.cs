using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.IdentityModel.Tokens;
using SistemaDigitalizacionPolizas.Application;
using SistemaDigitalizacionPolizas.Infrastructure;
using SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Service.Gmail_Services;
using SistemaDigitalizacionPolizas.API.SignalR;
using SistemaDigitalizacionPolizas.API.BackgroundWorkers;
using SistemaDigitalizacionPolizas.API.Extensions;
using System.Text;
using SistemaDigitalizacionPolizas.API.Middleware;

var builder = WebApplication.CreateBuilder(args);
var port = Environment.GetEnvironmentVariable("PORT") ?? "80";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

// ======================================================
// CONTROLLERS
// ======================================================
builder.Services.AddControllers();

// ======================================================
// SIGNALR
// ======================================================
builder.Services.AddSignalR();

// ======================================================
// TAMAÑO DE ARCHIVOS
// ======================================================
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 524288000;
});
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 524288000;
});

// ======================================================
// CORREOS (QUEUE + WORKER)
// ======================================================
builder.Services.AddSingleton<EmailQueue>();
builder.Services.AddSingleton<IEmailQueue>(sp => sp.GetRequiredService<EmailQueue>());
builder.Services.AddHostedService<EmailBackgroundWorker>();

// ======================================================
// HTTP CONTEXT
// ======================================================
builder.Services.AddHttpContextAccessor();

// ======================================================
// CORS
// ======================================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
// ======================================================
// SWAGGER (solo si está habilitado en config)
// ======================================================
builder.Services.AddSwaggerIfEnabled(builder.Configuration);

// ======================================================
// CAPAS (CLEAN ARCHITECTURE)
// ======================================================
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// ======================================================
// JWT
// ======================================================
builder.Services.AddJwtAuthentication(builder.Configuration);

// ======================================================
// AUTORIZACIÓN
// ======================================================
builder.Services.AddAuthorization();

// ======================================================
// BUILD
// ======================================================
var app = builder.Build();

// ======================================================
// MIDDLEWARE
// ======================================================
//app.UseMiddleware<ExceptionMiddleware>();
app.UseSwaggerIfEnabled(builder.Configuration);
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();

// ======================================================
// ENDPOINTS
// ======================================================
app.MapHub<NotificationHub>("/notifications");
app.MapControllers();

app.Run();