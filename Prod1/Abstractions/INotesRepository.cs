using btlz.Models;

namespace btlz.Abstractions;

public interface INotesRepository
{
    IEnumerable<Note> GetNotes();
    Note? GetNotesBy(Predicate<Note> predicate);
    int AddNotes(Note note);
    void UpdateNotes(Note note);
    void DeleteNotes(int id);
}