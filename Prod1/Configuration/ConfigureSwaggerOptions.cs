using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace btlz.Configuration;

public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    public void Configure(SwaggerGenOptions options)
    {
        options.AddSecurityDefinition(
            "Bearer", // Тип токена
            new OpenApiSecurityScheme // Схема для передачи токена в сваггере
            {
                In = ParameterLocation.Header, // Где будем передавать токен
                Description = "Please enter a valid token",
                Name = "Authorization", // Имя для заголовка
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT", // Формат для Bearer токена
                Scheme = JwtBearerDefaults.AuthenticationScheme
            });
        options.AddSecurityRequirement(
            new OpenApiSecurityRequirement
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
    }
}