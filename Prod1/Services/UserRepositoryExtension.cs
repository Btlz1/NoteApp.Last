using btlz.Abstractions;

namespace btlz.Services;

public static class UserRepositoryExtension
{
    public static IServiceCollection AddUserRepository(this IServiceCollection services)=>
    services.AddSingleton<IUserRepository, UserRepository>();
}