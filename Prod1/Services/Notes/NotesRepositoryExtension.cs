using btlz.Abstractions;

namespace btlz.Services;

public static class NotesRepositoryExtension
{
    public static IServiceCollection AddNotesRepository(this IServiceCollection services)=>
        services.AddScoped<INotesRepository, NotesRepository>();
}