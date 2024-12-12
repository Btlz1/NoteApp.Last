using System.Security.Claims;
using System.Text;
using btlz.Abstractions;
using btlz.Configuration;
using btlz.Politics;
using btlz.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.SwaggerGen;

public static class ServicesExtension
{
    public static IServiceCollection AddJwt(this IServiceCollection services)
    {
        services.AddTransient<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddSingleton<IJwtTokensRepository, JwtTokensRepository>();
        return services;
    }
    
    public static IServiceCollection AddConfiguredSwagger(
	    this IServiceCollection services)
    {
        services.AddSwaggerGen(); 
        services.AddTransient<IConfigureOptions<SwaggerGenOptions>,
	         ConfigureSwaggerOptions>(); 
        return services;
    }
    
    public static IServiceCollection AddAuth(
		   this IServiceCollection services,
	     IConfiguration configuration)
    {
        JwtSettings jwtSettings = new();
        configuration.Bind(nameof(JwtSettings), jwtSettings);
       
        services.AddSingleton(Options.Create(jwtSettings));
        
        services.AddAuthentication(defaultScheme: 
	        JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = true, 
                    ValidateAudience = true, 
                    ValidateLifetime = true, 
                    ValidateIssuerSigningKey = true, 
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
	                    Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                };
                options.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context => 
                    {
                        var tokensRepository =
                            context.HttpContext
	                            .RequestServices
	                            .GetRequiredService<IJwtTokensRepository>();
                        var userId = context.Principal?.FindFirst(
	                        ClaimTypes.NameIdentifier)?.Value;
                        if (
	                        userId is null
                            || context.SecurityToken.ValidTo < DateTime.UtcNow
                            || !tokensRepository.Verify(
	                            int.Parse(userId),
	                            context.SecurityToken.UnsafeToString()))
                        {
                            context.Fail("Unauthorized");
                        }
                        return Task.CompletedTask;
                    }
                };
            });
        services.AddScoped<IAuthorizationHandler, NoteOwnerRequirementHandler>();
        services.AddHttpContextAccessor();
        services.AddAuthorization(options =>
        {
            var defaultAuthorizationPolicyBuilder =
                new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme);
            defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
            options.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();

            // Добавляем политику проверки пользователя из токена
            options.AddPolicy("NotesOwner", policy =>
            {
                // Перед тем как проверять пользователя, отсекаем всех не аутентифицированных
                policy.RequireAuthenticatedUser();
                policy.AddRequirements(new NoteOwnerRequirement());
            });
        });
        return services;
    }
}