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
    
    public Note? GetNotesById(int id)
        => _dbContext.Notes.FirstOrDefault(note =>  note.Id == id);

    public int AddNotes(Note note, int usetId)
    {
        _dbContext.Notes.Add(note);
        note.DateCreated = DateTime.UtcNow;
        note.UserId = usetId;
        _dbContext.SaveChanges();
        return note.Id;
    }

    public void UpdateNotes(Note note)
    {
        var oldNotes = TryGetUserByIdAndThrowIfNotFound(note.Id);
        oldNotes.Name = note.Name;
        oldNotes.Description = note.Description;
        oldNotes.EditDate = DateTime.UtcNow;
        _dbContext.SaveChanges();
    }

    public void DeleteNotes(int id)
    {
        var notes = TryGetUserByIdAndThrowIfNotFound(id);
        _dbContext.Notes.Remove(notes);
        _dbContext.SaveChanges();
    }

    private Note TryGetUserByIdAndThrowIfNotFound(int id)
    {
        var note = _dbContext.Notes.FirstOrDefault(n => n.Id == id);
        if (note is null)
        {
            throw new UserNotFoundException(id);
        }
        return note;
    }
}