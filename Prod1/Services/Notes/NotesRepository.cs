using AutoMapper;
using btlz.Abstractions;
using btlz.Contracts;
using btlz.Exceptions;
using btlz.Models;
using btlz.Database;

namespace btlz.Services;

public class NotesRepository : INotesRepository
{
    private readonly btlzDbContext _dbContext;
    private readonly IMapper _mapper;
    public NotesRepository(btlzDbContext dbContext, IMapper mapper)
        => (_dbContext, _mapper) = (dbContext, mapper);

    public NotesVm GetNotes()
    {
        var listOfNotes = (
            from note in _dbContext.Notes
            select new NoteVm(note.Id, note.Name, note.Description)
        ).ToList();
        var notes = new NotesVm(listOfNotes);
        return notes;
    }
    
    public NotesVm GetNotesByUserId(int userId)
    {
        var listOfNotes = (
            from note in _dbContext.Notes
            where note.UserId == userId
            join user in _dbContext.Users on note.UserId equals user.Id
            select new NoteVm(note.Id, note.Name, note.Description)
        ).ToList();
        return new NotesVm(listOfNotes);
    }
    
    public int AddNotes(int userId, CreateNotesDto dto) 
    {
        _ = TryGetUserByIdAndThrowIfNotFound(userId);
        var note = _mapper.Map<Note>(dto);
        _dbContext.Notes.Add(note);
        _dbContext.SaveChanges();
        return note.Id;
    }

    public NoteVm UpdateNotes(int id, UpdateNotesDto dto)
    {
        var oldNotes = TryGetNotesByIdAndThrowIfNotFound(id);
        
        oldNotes.Name = dto.Name;
        oldNotes.Description = dto.Description;
        oldNotes.EditDate = DateTime.UtcNow;
        _dbContext.SaveChanges();
        return new NoteVm(oldNotes.Id, oldNotes.Name, oldNotes.Description);
    }
 
    public void DeleteNotes(int id)
    {
        var note = TryGetNotesByIdAndThrowIfNotFound(id);
        _dbContext.Notes.Remove(note);
        _dbContext.SaveChanges();
    }

    private Note TryGetNotesByIdAndThrowIfNotFound(int id)
    {
        var note = _dbContext.Notes.FirstOrDefault(n => n.Id == id);
        if (note is null)
        {
            throw new NotesNotFoundException(id);
        }
        return note;
    }
    private User TryGetUserByIdAndThrowIfNotFound(int id)
    {
        var user = _dbContext.Users.FirstOrDefault(u => u.Id == id);
        if (user is null)
        {
            throw new UserNotFoundException(id);
        }
        return user;
    }
}