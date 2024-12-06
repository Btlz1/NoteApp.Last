using System.Reflection;
using btlz.Database;
using btlz.Services;
using Microsoft.EntityFrameworkCore;

namespace WebANote3pplication1;

public static class Composer
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddDbContext<btlzDbContext>(options =>
        {
            options.UseNpgsql(
                "Host=localhost;Port=5432;Username=postgres;Password=Kosmos_12;Database=postgres"
            );
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