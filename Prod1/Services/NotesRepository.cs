using btlz.Abstractions;
using btlz.Exceptions;
using btlz.Models;
using btlz.Database;
using Microsoft.AspNetCore.Http.HttpResults;


namespace btlz.Services;

public class NotesRepository : INotesRepository
{
    private readonly btlzDbContext _dbContext;
    public NotesRepository(btlzDbContext dbContext)
        => _dbContext = dbContext;
    public IEnumerable<Note> GetNotes() => _dbContext.Notes;

    public IEnumerable<Note> GetNotesByUserId(int userId)
        => _dbContext.Notes.Where(review => review.UserId == userId);


    public int AddNotes(Note note, int userId)
    {
        var wrongUserId = TryGetNotesByUserIdAndThrowIfNotFound(userId);
        
        _dbContext.Notes.Add(note);
        note.DateCreated = DateTime.UtcNow;
        note.UserId = userId;
        _dbContext.SaveChanges();
        return note.Id;
    }

    public void UpdateNotes(int id, Note note)
    {
        
        var oldNotes = TryGetNotesByIdAndThrowIfNotFound(id);
        
        oldNotes.Name = note.Name;
        oldNotes.Description = note.Description;
        oldNotes.EditDate = DateTime.UtcNow;
        _dbContext.SaveChanges();
    }
 
    public void DeleteNotes(int id)
    {
        var note = TryGetNotesByIdAndThrowIfNotFound(id);
        _dbContext.Notes.Remove(note);
        _dbContext.SaveChanges();
    }

    private Note TryGetNotesByIdAndThrowIfNotFound(int id)
    {
        var note = _dbContext.Notes.FirstOrDefault(n => n.Id == id);
        if (note is null)
        {
            throw new NotesNotFoundException(id);
        }
        return note;
    }
    private User TryGetNotesByUserIdAndThrowIfNotFound(int id)
    {
        var user = _dbContext.Users.FirstOrDefault(u => u.Id == id);
        if (user is null)
        {
            throw new UserNotFoundException(id);
        }
        return user;
    }
}