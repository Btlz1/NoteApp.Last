using AutoMapper;
using btlz.Abstractions;
using btlz.Contracts;
using btlz.Exceptions;
using btlz.Models;
using btlz.Database;
using System.Linq;
using Microsoft.EntityFrameworkCore;


namespace btlz.Services;

public class NotesRepository : INotesRepository 
{
    private readonly btlzDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    private readonly CancellationTokenSource _cts;
    public NotesRepository(btlzDbContext dbContext, IMapper mapper, IUserRepository userRepository)
        => (_dbContext, _mapper, _userRepository) = (dbContext, mapper, userRepository);

    public async Task<List<NoteVm>> GetNotes(int userId)
    {
        using var cts = new CancellationTokenSource();
        cts.CancelAfter(5000);
        var token = cts.Token;
        
        var listOfNotes = await _dbContext.Notes
            .Where(note => note.UserId == userId)
            .Select(note => new NoteVm(note.UserId, note.Id, note.Name, note.Description, note.Tags))
            .ToListAsync(token);

        return listOfNotes;
    }
    
    public async Task<Note> AddNotes(Note dto)
    {
        using var cts = new CancellationTokenSource();
        cts.CancelAfter(5000);
        var token = cts.Token;
        
        var  note = _mapper.Map<Note>(dto);
        await _dbContext.Notes.AddAsync(note, token);
        await _dbContext.SaveChangesAsync(token);
        return note;
    }

    public async Task<int> UpdateNotes(int id, UpdateNotesDto dto)
    {
        using var cts = new CancellationTokenSource();
        cts.CancelAfter(5000);
        var token = cts.Token;
        
        var note = TryGetNotesByIdAndThrowIfNotFound(id);
        var updatedNote = _mapper.Map<(int, UpdateNotesDto), Note>((id, dto));
        note.Name = updatedNote.Name;
        note.Description = updatedNote.Description;
        note.Finished = true;
        note.EditDate = DateTime.UtcNow;
        note.Tags = updatedNote.Tags;
        await _dbContext.SaveChangesAsync(token);
        return note.Id;
    }
 
    public async Task DeleteNotes(int id)
    {
        using var cts = new CancellationTokenSource();
        cts.CancelAfter(5000);
        var token = cts.Token;
        
        var note = TryGetNotesByIdAndThrowIfNotFound(id);
        _dbContext.Notes.Remove(note);
        await _dbContext.SaveChangesAsync(token);
    }
    
    public async Task<List<NoteVm>> FilteredByTags(Tag tag, int userId)
    {
        using var cts = new CancellationTokenSource();
        cts.CancelAfter(5000);
        var token = cts.Token;
        
        var filteredNotes =await _dbContext.Notes
            .Where(note => note.UserId == userId && note.Tags.HasValue && note.Tags.Value == tag) 
            .Select(note => new NoteVm(note.UserId, note.Id, note.Name, note.Description, note.Tags)) 
            .ToListAsync(token); 

        return filteredNotes;
    }

    public async Task<List<NoteVm>> SortedByTags(Tag tag, int userId)
    {
        using var cts = new CancellationTokenSource();
        cts.CancelAfter(5000);
        var token = cts.Token;

        var sortedNotes =await _dbContext.Notes
            .Where(note => note.UserId == userId) 
            .OrderByDescending(note => note.UserId == userId && note.Tags.HasValue && note.Tags.Value == tag)
            .ThenBy(note => note.Tags == null ? 1 : 0) 
            .ThenBy(note => note.Name) 
            .Select(note => new NoteVm(note.UserId, note.Id, note.Name, note.Description, note.Tags)) 
            .ToListAsync(token); 

        return sortedNotes;
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
}