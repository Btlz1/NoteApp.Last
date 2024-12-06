using btlz.Models;

namespace btlz.Abstractions;

public interface INotesRepository
{
    IEnumerable<Notes> GetNotes();
    Notes? GetNotesBy(Predicate<Notes> predicate);
    int AddNotes(Notes notes);
    void UpdateNotes(Notes notes);
    void DeleteNotes(int id);
}