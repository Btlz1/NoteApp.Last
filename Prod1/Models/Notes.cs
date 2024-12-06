namespace btlz.Models;

public class Notes
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? EditDate {get; set;}
    public bool? Finished  { get; set; } 
    public int UserId {get; set;} 
    public Priority? Priority { get; set; }
}