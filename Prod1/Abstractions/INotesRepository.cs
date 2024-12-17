using btlz.Contracts;
using btlz.Models;

namespace btlz.Abstractions;

public interface INotesRepository
{
    Task<List<NoteVm>> GetNotes(int userId);
    Task<Note> AddNotes(Note dto);
    Task<int> UpdateNotes(int id, UpdateNotesDto dto);
    Task DeleteNotes(int id);
    Task<List<NoteVm>> SortedByTags(Tag tag, int userId);
    Task<List<NoteVm>> FilteredByTags(Tag tag, int userId);
}