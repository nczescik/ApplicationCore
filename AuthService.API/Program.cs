using AuthService.API.Extensions;
using AuthService.Infrastructure;
using AuthService.Settings;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var jwtSection = builder.Configuration.GetSection("JwtSettings");

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerExt()
    .AddDbContext<AuthDbContext>(options =>
        options.UseInMemoryDatabase("AuthServiceDatabase"))
    .AddDependencyInjection()
    .AddRabbitMq(builder)
    .Configure<JwtSettings>(jwtSection)
    .AddAuthenticationExt(jwtSection.Key)
    .AddControllers();

var app = builder.Build();

app.UseAuthentication()
    .UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwaggerExt();
}

app.MapControllers();

app.Run();
