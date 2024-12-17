using btlz.Contracts;
using btlz.Models;

namespace btlz.Abstractions;

public interface INotesRepository
{
    Task<List<NoteVm>> GetNotes(int userId);
    Task<Note> AddNotes(Note dto);
    Task<int> UpdateNotes(int id, UpdateNotesDto dto);
    Task DeleteNotes(int id);
    Task<List<NoteVm>> SortedByTags(Enum tags, int userId);
    Task<List<NoteVm>> FilteredByTags(Enum tags, int userId);
}