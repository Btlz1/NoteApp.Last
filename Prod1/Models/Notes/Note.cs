namespace btlz.Models;

public class Note
{
    public int Id { get;}
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? EditDate {get; set;}
    public bool? Finished { get; set; } 
    public int UserId { get; set; }
    public Tag? Tags  { get; set; } 
}

public enum Tag : byte
{
    Red = 1,
    Green,
    Blue
}