using btlz.Abstractions;
using btlz.Exceptions;
using btlz.Models;

namespace btlz.Services;

public class NotesRepository : INotesRepository
{
    private static readonly List<Notes> _notes = new()
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
    
    public IEnumerable<Notes> GetNotes() => _notes;
    
    public Notes? GetNotesBy(Predicate<Notes> predicate)
        => _notes.FirstOrDefault(note =>  predicate(note));

    public int AddNotes(Notes notes)
    {
        var notesId = _notes.Count;
        _notes.Add(new Notes
        {
            Id = notesId,
            Name = notes.Name,
            DateCreated = DateTime.UtcNow,
            Description = notes.Description,
            UserId = notes.UserId
        });
        return notesId;
    }

    public void UpdateNotes(Notes notes)
    {
        var oldNotes = TryGetUserByIdAndThrowIfNotFound(notes.Id);
        oldNotes.Name = notes.Name;
        oldNotes.Description = notes.Description;
        oldNotes.EditDate = DateTime.UtcNow;
    }

    public void DeleteNotes(int id)
    {
        var notes = TryGetUserByIdAndThrowIfNotFound(id);
        _notes.Remove(notes);
    }

    private Notes TryGetUserByIdAndThrowIfNotFound(int id)
    {
        var notes = _notes.FirstOrDefault(n => n.Id == id);
        if (notes is null)
        {
            throw new UserNotFoundException(id);
        }
        return notes;
    }
}