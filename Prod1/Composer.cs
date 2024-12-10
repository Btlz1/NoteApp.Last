using System.Reflection;
using btlz.Database;
using btlz.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using WebApplication1.Configuration.DataBase;

namespace WebApplication1;

public static class Composer
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.Configure<btlzDbConnectionSettings>(
            configuration.GetRequiredSection(nameof(btlzDbConnectionSettings)));
        services.AddDbContext<btlzDbContext>(options =>
        {
            using var scope = services.BuildServiceProvider().CreateScope();
            var settings = scope.ServiceProvider.GetRequiredService<IOptions<btlzDbConnectionSettings>>().Value;
            options.UseNpgsql(settings.ConnectionString);
        });
        return services;
    }
    
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        return services;
    }

    public static IServiceCollection AddApplicationServices(this
        IServiceCollection services)
    {
        services.AddExceptionHandler<ExceptionHandler>();
        services.AddUserRepository();
        services.AddNotesRepository();
        return services;
    }
}