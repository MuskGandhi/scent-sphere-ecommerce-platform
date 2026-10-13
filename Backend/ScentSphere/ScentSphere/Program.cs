using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ScentSphere.Application.Common;
using ScentSphere.Application.Interfaces;
using ScentSphere.Application.Services;
using ScentSphere.Domain.Entities;
using ScentSphere.Filters;
using ScentSphere.Infrastructure.Auth;
using ScentSphere.Infrastructure.Email;
using ScentSphere.Infrastructure.Security;
using ScentSphere.Middleware;
using ScentSphere.Persistence.Context;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Logging
builder.Logging.AddConsole();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularPolicy", policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithOrigins(
                "http://localhost:4200",
                "https://scentsphere-ui.onrender.com"
            );
    });
});

// Controllers
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidateModelAttribute>();
});

// Swagger
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "ScentSphere API",
            Version = "v1"
        });

    options.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description =
                "Enter JWT token. Example: Bearer eyJhbGciOi..."
        });

    options.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference =
                        new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                },
                Array.Empty<string>()
            }
        });
});

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString(
            "DefaultConnection"));
});

// JWT Validation

var jwtKey = builder.Configuration["Jwt:Key"];

if (string.IsNullOrWhiteSpace(jwtKey))
{
    throw new InvalidOperationException(
        "JWT Key is not configured.");
}

builder.Services.AddAuthentication(
    JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters =
        new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer =
                builder.Configuration["Jwt:Issuer"],

            ValidAudience =
                builder.Configuration["Jwt:Audience"],

            IssuerSigningKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtKey))
        };
});

builder.Services.AddAuthorization();

// Services
builder.Services.AddScoped<
    IJwtTokenService,
    JwtTokenService>();

builder.Services.AddScoped<
    IPasswordHasher,
    PasswordHasher>();

builder.Services.AddScoped<
    IAuthService,
    AuthService>();

builder.Services.AddScoped<
    IProductService,
    ProductService>();

builder.Services.AddScoped<
    ICategoryService,
    CategoryService>();

builder.Services.AddScoped<
    ICartService,
    CartService>();

builder.Services.AddScoped<
    IOrderService,
    OrderService>();

builder.Services.AddScoped<
    IEmailService,
    SmtpEmailService>();

builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection(
        "EmailSettings"));

var app = builder.Build();

// Swagger available everywhere
app.UseSwagger();
app.UseSwaggerUI();

// Global Exception Middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseCors("AngularPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

//
// Seed Data
//
try
{
    using var scope =
        app.Services.CreateScope();

    var db =
        scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

    if (db.Database.CanConnect())
    {
        if (!db.Brands.Any())
        {
            db.Brands.AddRange(
                new Brand
                {
                    Id = Guid.NewGuid(),
                    Name = "Dior",
                    Description = "Dior Perfumes"
                },
                new Brand
                {
                    Id = Guid.NewGuid(),
                    Name = "Chanel",
                    Description = "Chanel Perfumes"
                });

            db.SaveChanges();
        }

        if (!db.Categories.Any())
        {
            db.Categories.AddRange(
                new Category
                {
                    Id = Guid.NewGuid(),
                    Name = "Men"
                },
                new Category
                {
                    Id = Guid.NewGuid(),
                    Name = "Women"
                });

            db.SaveChanges();
        }
    }
}
catch (Exception ex)
{
    app.Logger.LogError(
        ex,
        "Database initialization failed.");
}

app.Run();