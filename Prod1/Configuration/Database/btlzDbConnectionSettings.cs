namespace WebApplication1.Configuration.DataBase;

public class btlzDbConnectionSettings
{
    public string Host { get; set; } = default!;
    public int Port { get; set; }
    public string Username { get; set; } = default!;
    public string Password { get; set; } = default!;
    public string Database { get; set; } = default!;
    
    public string ConnectionString 
        => $"Host={Host};Port={Port};Username={Username};Password={Password};Database={Database}";
}