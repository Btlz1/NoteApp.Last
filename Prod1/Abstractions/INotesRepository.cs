using btlz.Contracts;
using btlz.Models;

namespace btlz.Abstractions;

public interface INotesRepository
{
    List<NoteVm> GetNotes(int userId);
    Note AddNotes(Note dto);
    int UpdateNotes(int id, UpdateNotesDto dto);
    void DeleteNotes(int id);
    List<NoteVm> SortedByTags(Enum tags, int userId);
    List<NoteVm> FilteredByTags(Enum tags, int userId);
}