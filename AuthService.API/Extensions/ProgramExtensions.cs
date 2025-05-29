using AuthService.API.Security;
using AuthService.Application.Authentication.Commands.Login;
using AuthService.Application.User.Queries.GetUser;
using AuthService.Infrastructure;
using AuthService.Infrastructure.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using Shared.Infrastructure.Messaging.Outbox;
using Shared.Infrastructure.Messaging.Outbox.Repository;
using Shared.Infrastructure.Messaging.RabbitMQ;
using System.Text;


namespace AuthService.API.Extensions
{
    public static class ProgramExtensions
    {

        public static IServiceCollection AddSwaggerExt(this IServiceCollection services)
        {
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        new string[]{}
                    }
                });
            });
            return services;
        }

        public static IServiceCollection AddAuthenticationExt(this IServiceCollection services, string key)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            return services;
        }

        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            services
                .AddScoped<IOutboxDbContext>(provider => provider.GetRequiredService<AuthDbContext>())
                .AddScoped<IJwtTokenGenerator, JwtTokenGenerator>()
                .AddScoped<IEventPublisher, RabbitMqEventPublisher>()
                .AddScoped<IOutboxRepository, OutboxRepository>()
                .AddScoped<IUserRepository, UserRepository>();

            return services;
        }
        public static IServiceCollection AddMediatRExt(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssemblyContaining<LoginCommand>();
                cfg.RegisterServicesFromAssemblyContaining<GetUserQuery>();
            });
            return services;
        }

        public static IServiceCollection AddRabbitMq(this IServiceCollection services, WebApplicationBuilder builder)
        {
            services.AddSingleton<IConnectionFactory>(sp => new ConnectionFactory
            {
                HostName = builder.Configuration["RabbitMQ:Host"] ?? "localhost"
            });
            return services;
        }
        public static IApplicationBuilder UseSwaggerExt(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
                options.RoutePrefix = string.Empty;
            });
            return app;
        }
    }
}
