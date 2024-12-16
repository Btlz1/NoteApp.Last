namespace btlz.Models;

public class User
{
    public int Id { get; }
    public string Login { get; set; } = null!;
    public string Password { get; set; } = null!;
}