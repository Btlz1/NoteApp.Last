namespace btlz.Exceptions;

public class NotesNotFoundException: Exception
{
    public NotesNotFoundException(int id) : base($"Notes with id = {id} not found") { }
}