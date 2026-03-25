using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SistemaDigitalizacionPolizas.Application;
using SistemaDigitalizacionPolizas.Infrastructure;
using SistemaDigitalizacionPolizas.Application.Services.ExpedientDocument_Service.Service.Gmail_Services;



using System.Text;
using SistemaDigitalizacionPolizas.API.BackgroundWorkers.SistemaDigitalizacionPolizas.API.BackgroundWorkers;
using SistemaDigitalizacionPolizas.API.SignalR;

var builder = WebApplication.CreateBuilder(args);


// ======================================================
// CONTROLLERS
// ======================================================

builder.Services.AddControllers();


// ======================================================
// SIGNALR (NOTIFICACIONES EN TIEMPO REAL)
// ======================================================

builder.Services.AddSignalR();


// ======================================================
// CONFIGURACIÓN DE TAMAÑO DE ARCHIVOS
// ======================================================

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 524288000; // 500 MB
});

builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 524288000; // 500 MB
});


// ======================================================
// SERVICIO DE CORREOS (QUEUE + WORKER)
// ======================================================

// Cola de correos
builder.Services.AddSingleton<EmailQueue>();

// Interfaz de la cola
builder.Services.AddSingleton<IEmailQueue>(sp =>
    sp.GetRequiredService<EmailQueue>());

// Worker que procesa la cola
builder.Services.AddHostedService<EmailBackgroundWorker>();


// ======================================================
// HTTP CONTEXT (para CurrentUserService)
// ======================================================

builder.Services.AddHttpContextAccessor();


// ======================================================
// CORS (FRONTEND)
// ======================================================

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:5173", 
                "http://localhost:3000"  
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); 
    });
});


// ======================================================
// SWAGGER
// ======================================================

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SDPE API",
        Version = "v1"
    });

    // JWT en Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingrese el token JWT como: Bearer {token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


// ======================================================
// CAPAS DE LA APLICACIÓN (CLEAN ARCHITECTURE)
// ======================================================

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);


// ======================================================
// AUTENTICACIÓN JWT
// ======================================================

var jwtSettings = builder.Configuration.GetSection("Jwt");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],

        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSettings["Key"]!)
        ),

        ClockSkew = TimeSpan.Zero
    };

    // NECESARIO PARA QUE SIGNALR FUNCIONE CON JWT
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Query["access_token"];

            var path = context.HttpContext.Request.Path;

            if (!string.IsNullOrEmpty(accessToken) &&
                path.StartsWithSegments("/notifications"))
            {
                context.Token = accessToken;
            }

            return Task.CompletedTask;
        }
    };
});


// ======================================================
// AUTORIZACIÓN
// ======================================================

builder.Services.AddAuthorization();


// ======================================================
// BUILD APP
// ======================================================

var app = builder.Build();


// ======================================================
// MIDDLEWARE PIPELINE
// ======================================================

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthentication();

app.UseAuthorization();


// ======================================================
// ENDPOINT DE SIGNALR
// ======================================================

app.MapHub<NotificationHub>("/notifications");


// ======================================================
// CONTROLLERS
// ======================================================

app.MapControllers();

app.Run();