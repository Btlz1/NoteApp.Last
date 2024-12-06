using btlz.Abstractions;
using btlz.Exceptions;
using btlz.Models;

namespace btlz.Services;

public class NotesRepository : INotesRepository
{
    private static readonly List<Note> _notes = new()
    {
        new()
        {
            Id = 0,
            Name = "Первая заметка",
            Description = "Моя самая первая заметочка, даже не знаю как ее описать. Ура Ура!",
            DateCreated = DateTime.UtcNow,
            UserId = 0
        },
    };
    
    public IEnumerable<Note> GetNotes() => _notes;
    
    public Note? GetNotesBy(Predicate<Note> predicate)
        => _notes.FirstOrDefault(note =>  predicate(note));

    public int AddNotes(Note note)
    {
        var notesId = _notes.Count;
        _notes.Add(new Note
        {
            Id = notesId,
            Name = note.Name,
            DateCreated = DateTime.UtcNow,
            Description = note.Description,
            UserId = note.UserId
        });
        return notesId;
    }

    public void UpdateNotes(Note note)
    {
        var oldNotes = TryGetUserByIdAndThrowIfNotFound(note.Id);
        oldNotes.Name = note.Name;
        oldNotes.Description = note.Description;
        oldNotes.EditDate = DateTime.UtcNow;
    }

    public void DeleteNotes(int id)
    {
        var notes = TryGetUserByIdAndThrowIfNotFound(id);
        _notes.Remove(notes);
    }

    private Note TryGetUserByIdAndThrowIfNotFound(int id)
    {
        var notes = _notes.FirstOrDefault(n => n.Id == id);
        if (notes is null)
        {
            throw new UserNotFoundException(id);
        }
        return notes;
    }
}