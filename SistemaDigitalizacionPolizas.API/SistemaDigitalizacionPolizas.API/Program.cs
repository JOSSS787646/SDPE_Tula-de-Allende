using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SistemaDigitalizacionPolizas.Application;
using SistemaDigitalizacionPolizas.Infrastructure;
using System.Text;



var builder = WebApplication.CreateBuilder(args);

// =======================
// Controllers
// =======================
builder.Services.AddControllers();


builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 524288000; // 500 MB total por request
});

// 🔥 NECESARIO para CurrentUserService
builder.Services.AddHttpContextAccessor();

// =======================
// CORS (Frontend)
// =======================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:5173", // Vite
                "http://localhost:3000"  // CRA
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// =======================
// Swagger
// =======================
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

// =======================
// Capas (Clean Architecture)
// =======================
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);


// =======================
// JWT Authentication
// =======================
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
    });


builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 524288000; // 500 MB
});

// =======================
// Authorization
// =======================

builder.Services.AddAuthorization();

var app = builder.Build();
//app.UseMiddleware<ExceptionMiddleware>();
// =======================
// Pipeline
// =======================
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
