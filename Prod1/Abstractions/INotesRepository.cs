using btlz.Models;

namespace btlz.Abstractions;

public interface INotesRepository
{
    IEnumerable<Note> GetNotes();
    IEnumerable<Note> GetNotesByUserId(int userId);
    int AddNotes(Note note, int userId);
    void UpdateNotes(int id, Note note);
    void DeleteNotes(int id);
}