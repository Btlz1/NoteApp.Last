using btlz.Abstractions;
using btlz.Exceptions;
using btlz.Models;
using btlz.Database;
using btlz.Controllers;

namespace btlz.Services;

public class NotesRepository : INotesRepository
{
    private readonly btlzDbContext _dbContext;

    public NotesRepository(btlzDbContext dbContext)
        => _dbContext = dbContext;
    public IEnumerable<Note> GetNotes() => _dbContext.Notes;

    public IEnumerable<Note> GetNotesByUserId(int id)
    {
        var userId = TryGetNotesByUserIdAndThrowIfNotFound(id);
        
        var notes = _dbContext.Notes.Where(note => note.UserId == id).ToList(); 

        return notes; 
    }


    public int AddNotes(Note note, int userId)
    {
        var tut = TryGetNotesByUserIdAndThrowIfNotFound(userId);
        
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
        var notes = TryGetNotesByIdAndThrowIfNotFound(id);
        _dbContext.Notes.Remove(notes);
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
        var user = _dbContext.Users.FirstOrDefault(n => n.Id == id);
        if (user is null)
        {
            throw new UserNotFoundException(id);
        }
        return user;
    }
}