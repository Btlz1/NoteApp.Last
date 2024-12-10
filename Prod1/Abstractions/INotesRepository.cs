using btlz.Contracts;
using btlz.Models;

namespace btlz.Abstractions;

public interface INotesRepository
{
    NotesVm GetNotes();
    NotesVm GetNotesByUserId(int userId);
    int AddNotes(int userId, CreateNotesDto dto);
    int UpdateNotes(int id, UpdateNotesDto dto);
    void DeleteNotes(int id);
}