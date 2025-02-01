using Api.Services;
using Application.Constants;
using Application.DTOs;
using Application.DTOs.Settings;
using Application.Interfaces.Identity;
using Application.Interfaces.Shared;
using Application.Providers;
using Infrastructure.DbContext;
using Infrastructure.Identity.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Text;
using Domain.Entities;

namespace Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddEssentials(this IServiceCollection services)
        {
            services.RegisterSwagger();
            services.AddVersioning();
            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
        }

        public static void AddContextInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("Default"), b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddIdentity<User, IdentityRole<int>>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequireNonAlphanumeric = false;
                options.User.RequireUniqueEmail = true;
                options.Lockout.MaxFailedAccessAttempts = 3;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
            }).AddEntityFrameworkStores<ApplicationDbContext>();

            #region Services

            services.TryAddTransient<IAuthenticatedUserService, AuthenticatedUserService>();
            services.TryAddTransient<IIdentityService, IdentityService>();

            #endregion Services

            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = configuration["JwtSettings:Issuer"],
                        ValidAudience = configuration["JwtSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]!))
                    };
                    string result = string.Empty;
                    o.Events = new JwtBearerEvents()
                    {
                        OnAuthenticationFailed = context =>
                        {
                            if (!context.Response.HasStarted)
                            {
                                context.Response.ContentType = "application/json";
                                string result = string.Empty;

                                // Mapeo de códigos de estado y mensajes basados en la excepción
                                context.Response.StatusCode = context.Exception switch
                                {
                                    SecurityTokenExpiredException => 401,
                                    SecurityTokenInvalidSignatureException => 401,
                                    SecurityTokenInvalidAudienceException => 401,
                                    SecurityTokenInvalidIssuerException => 401,
                                    SecurityTokenNoExpirationException => 401,
                                    SecurityTokenNotYetValidException => 401,
                                    SecurityTokenMalformedException => 401,
                                    _ => 500
                                };

                                string message = context.Exception switch
                                {
                                    SecurityTokenExpiredException => "La sesión ha expirado. Inicie sesión nuevamente",
                                    SecurityTokenInvalidSignatureException => "La firma del token no es válida",
                                    SecurityTokenInvalidAudienceException => "El token no está destinado para esta audiencia",
                                    SecurityTokenInvalidIssuerException => "El emisor del token no es válido",
                                    SecurityTokenNoExpirationException => "El token no tiene fecha de expiración",
                                    SecurityTokenNotYetValidException => "El token aún no es válido",
                                    SecurityTokenMalformedException => "El formato del token es erróneo",
                                    _ => "Error de autenticación"
                                };

                                result = JsonConvert.SerializeObject(
                                    Result<string>.Unauthorized(message,
                                    [new ErrorResult(context.Exception.Message, context.Exception.InnerException?.Message)]),
                                    ServerOptions.JsonSettings
                                );
                                return context.Response.WriteAsync(result);
                            }
                            return Task.CompletedTask;
                        },
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            if (!context.Response.HasStarted)
                            {
                                context.Response.StatusCode = 401;
                                context.Response.ContentType = "application/json";
                                result = JsonConvert.SerializeObject(Result<string>.Unauthorized("Acceso no autorizado, autenticación requerida"), ServerOptions.JsonSettings);
                                return context.Response.WriteAsync(result);
                            }
                            return Task.CompletedTask;
                        },
                        OnForbidden = context =>
                        {
                            if (!context.Response.HasStarted)
                            {
                                context.Response.StatusCode = 403;
                                context.Response.ContentType = "application/json";
                                var result = JsonConvert.SerializeObject(Result<string>.Forbidden("No tienes permiso para acceder a este recurso"), ServerOptions.JsonSettings);
                                return context.Response.WriteAsync(result);
                            }
                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            // Aquí puedes agregar lógica adicional si deseas, como verificar otros parámetros del token.
                            return Task.CompletedTask;
                        }
                    };
                });
        }

        private static void AddVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
                config.ErrorResponses = new CustomErrorResponseProvider();
            });
        }
        private static void RegisterSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "CleanTemplate",
                    License = new OpenApiLicense()
                    {
                        Name = "Copyright - Derechos reservados"
                    }
                });

                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    Description = "Por favor, ingrese el token generado aquí",
                    Reference = new OpenApiReference { Id = JwtBearerDefaults.AuthenticationScheme, Type = ReferenceType.SecurityScheme }
                };
                c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement { { jwtSecurityScheme, Array.Empty<string>() } });
            });
        }
    }
}
