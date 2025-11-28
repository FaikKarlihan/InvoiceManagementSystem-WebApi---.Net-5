using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text;
using WebApi.Repositories;
using WebApi.Services;
using WebApi.Data;
using WebApi.Authentication;
using Microsoft.AspNetCore.Identity;
using WebApi.Entities;
using WebApi.Common;

namespace WebApi.Extensions
{
    public static class ServiceRegistration
    {
        public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // JWT Authentication
            var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);

            services.AddAuthentication("Bearer")
                .AddJwtBearer(options =>
                {
                    options.MapInboundClaims = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ClockSkew = TimeSpan.Zero,
                    };
                });

            // JWT Helper
            services.AddScoped<TokenGenerator>();

            // Password Hasher
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

            // Authorization
            services.AddAuthorization();
            services.AddHttpContextAccessor();

            // AutoMapper
            services.AddAutoMapper(typeof(MappingProfile));

            // Database   // Eğer Connection strin verilmediyse inmemory db ile test edilebilir yapı
            if(string.IsNullOrWhiteSpace(configuration.GetConnectionString("DefaultConnection")))
                services.AddDbContext<ImsDbContext>(options => options.UseInMemoryDatabase(databaseName:"InvoiceManagementSystemDB")); //MOCK DATA SEED EKLE !!!
            else
                services.AddDbContext<ImsDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IImsDbContext>(provider => provider.GetRequiredService<ImsDbContext>());

            // Logger
            services.AddSingleton<IloggerService, ConsoleLogger>();

            // Configurations
            services.Configure<JwtSettings>(configuration.GetSection("Jwt"));

            // Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApi", Version = "v1" });

                // Activates the [Tags] and [SwaggerOperation] attributes
                c.EnableAnnotations();
                
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            // Repositories
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IHousingRepository, HousingRepository>();
            services.AddScoped<IInvoiceRepository, InvoiceRepository>();

            // Services
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IInvoiceService, InvoiceService>();
            services.AddScoped<IHousingService, HousingService>();
        }
    }
}
