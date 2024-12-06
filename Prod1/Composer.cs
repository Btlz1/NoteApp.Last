using System.Reflection;
using btlz.Services;

namespace WebApplication1;

public static class Composer
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
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