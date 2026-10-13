using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ScentSphere.Application.Common;
using ScentSphere.Middleware;
using ScentSphere.Filters;
using ScentSphere.Application.Interfaces;
using ScentSphere.Application.Services;
using ScentSphere.Domain.Entities;
using ScentSphere.Infrastructure.Auth;
using ScentSphere.Infrastructure.Email;
using ScentSphere.Infrastructure.Security;
using ScentSphere.Persistence.Context;
using System.Text;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
// ensure console logging is available for development and tests
builder.Logging.AddConsole();

// Add services to the container.
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularPolicy",
        policy =>
        {
            policy
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .WithOrigins(
                    "http://localhost:4200");
        });
});
builder.Services.AddControllers(options =>
{
    // global model validation filter
    options.Filters.Add<ValidateModelAttribute>();
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
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
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<
    IJwtTokenService,
    JwtTokenService>();

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
                    Encoding.UTF8.GetBytes(
                        builder.Configuration["Jwt:Key"]!))
        };
});

builder.Services.AddAuthorization();

builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProductService,ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IEmailService, SmtpEmailService>();
builder.Services.Configure<EmailSettings>(
builder.Configuration.GetSection("EmailSettings"));



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// centralized exception handling middleware (non-invasive)
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseHttpsRedirection();
app.UseCors("AngularPolicy");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var db =
        scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

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

app.Run();
