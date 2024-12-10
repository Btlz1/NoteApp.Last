using btlz.Contracts;
using btlz.Models;

namespace btlz.Abstractions;

public interface INotesRepository
{
    NotesVm GetNotes();
    NotesVm GetNotesByUserId(int userId);
    NoteVm AddNotes(int userId, CreateNotesDto notes);
    NoteVm UpdateNotes(int id, UpdateNotesDto notes);
    void DeleteNotes(int id);
}