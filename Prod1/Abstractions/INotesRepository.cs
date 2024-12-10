using btlz.Contracts;
using btlz.Models;

namespace btlz.Abstractions;

public interface INotesRepository
{
    NotesVm GetNotes();
    NotesVm GetNotesByUserId(int userId);
    int AddNotes(int userId, CreateNotesDto dto);
    NoteVm UpdateNotes(int id, UpdateNotesDto notes);
    void DeleteNotes(int id);
}