using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Minio;
using System.Reflection;
using System.Text;
using Telegram.Bot;
using WebApp.Aplication.Common;
using WebApp.Aplication.Helpers.GenerateJWT;
using WebApp.Aplication.Helpers.PasswordHash;
using WebApp.Aplication.Models;
using WebApp.Aplication.Models.Users;
using WebApp.Aplication.Services;
using WebApp.Aplication.Services.Impl;
using WebApp.Aplication.Services.Interface;
using WebApp.Aplication.Validators;
using WebApp.DataAccess.Persistence;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            var jwtSettings = builder.Configuration.GetSection("JwtOption").Get<JwtOption>()
                ?? throw new InvalidOperationException("JwtOption configuration section is missing");

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
                };
            });

            builder.Services.AddControllers();

            // CORS configuration for frontend (development - allow all origins)
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            builder.Services.AddDbContext<AppDbContext>(option =>
            option.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.Configure<JwtOption>(configuration.GetSection("JwtOption"));
            builder.Services.Configure<EmailConfiguration>(configuration.GetSection("EmailConfiguration"));
            builder.Services.Configure<MinioSettings>(configuration.GetSection("MinioSettings"));

            // ���� �������
            builder.Services.AddScoped<IFileStorageService, MinioFileStorageService>();
            builder.Services.AddScoped<ITableService, TableService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IOtpService, OtpService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
            builder.Services.AddScoped<Helper>();
            builder.Services.AddScoped<PasswordHash>();
            builder.Services.AddScoped<JwtService>();

            // ���������� - ����� �����������
            builder.Services.AddScoped<IValidator<UserRegistrDTO>, UserRegistrDTOValidator>();
            builder.Services.AddScoped<IValidator<UserUpdateDTO>, UserUpdateDTOValidator>();
            builder.Services.AddScoped<IValidator<ChangePassword>, ChangePasswordValidator>();

            // TELEGRAM BOT CLIENT
            builder.Services.AddSingleton<ITelegramBotClient>(sp =>
            {
                var token = configuration["TelegramBot:Token"];
                if (string.IsNullOrEmpty(token))
                {
                    throw new InvalidOperationException("Telegram bot token �� ������ � ������������");
                }
                return new TelegramBotClient(token);
            });

            // TELEGRAM BOT - ��� Hosted Service
            builder.Services.AddHostedService<RestaurantTelegramBot>();

            // Minio clientni registr qilish
            builder.Services.AddSingleton<IMinioClient>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<MinioSettings>>().Value;

                return new MinioClient()
                    .WithEndpoint(settings.Endpoint)
                    .WithCredentials(settings.AccessKey, settings.SecretKey)
                    .WithSSL(settings.UseSsl)
                    .Build();
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "RestaurantApp", Version = "v1" });

                var securitySchema = new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };

                c.AddSecurityDefinition("Bearer", securitySchema);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        securitySchema, new[] { "Bearer" }
                    }
                });
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseHttpsRedirection();
            }

            app.UseCors("AllowFrontend");
            app.UseAuthentication();
            app.UseAuthorization();

            // MINIMAL API
            app.MapGet("/hello", () => "salom dunyo").WithName("GetHello");
            app.MapGet("/hello/{name}", (string name) => $"Salom {name} jigar")
                .WithName("GetHelloWithName");

            app.MapControllers();

            app.Run();
        }
    }
}