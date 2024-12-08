using btlz.Models;

namespace btlz.Abstractions;

public interface INotesRepository
{
    IEnumerable<Note> GetNotes();
    Note? GetNotesById(int id);
    int AddNotes(Note note, int userId);
    void UpdateNotes(Note note);
    void DeleteNotes(int id);
}