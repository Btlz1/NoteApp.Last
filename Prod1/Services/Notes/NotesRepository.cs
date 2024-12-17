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
    public NotesRepository(btlzDbContext dbContext, IMapper mapper, IUserRepository userRepository)
        => (_dbContext, _mapper, _userRepository) = (dbContext, mapper, userRepository);

    public async Task<List<NoteVm>> GetNotes(int userId)
    {
        var listOfNotes = await _dbContext.Notes
            .Where(note => note.UserId == userId)
            .Select(note => new NoteVm(note.UserId, note.Id, note.Name, note.Description, note.Tags))
            .ToListAsync();

        return listOfNotes;
    }
    
    public async Task<Note> AddNotes(Note dto)
    {
        var  note = _mapper.Map<Note>(dto);
        await _dbContext.Notes.AddAsync(note);
        await _dbContext.SaveChangesAsync();
        return note;
    }

    public async Task<int> UpdateNotes(int id, UpdateNotesDto dto)
    {
        var note = TryGetNotesByIdAndThrowIfNotFound(id);
        var updatedNote = _mapper.Map<(int, UpdateNotesDto), Note>((id, dto));
        note.Name = updatedNote.Name;
        note.Description = updatedNote.Description;
        note.Finished = true;
        note.EditDate = DateTime.UtcNow;
        note.Tags = updatedNote.Tags;
        await _dbContext.SaveChangesAsync();
        return note.Id;
    }
 
    public async Task DeleteNotes(int id)
    {
        var note = TryGetNotesByIdAndThrowIfNotFound(id);
        _dbContext.Notes.Remove(note);
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task<List<NoteVm>> FilteredByTags(Enum tags, int userId)
    {
        var enumTagValue = Convert.ToByte(tags); // Преобразуем Enum в его числовое значение

        var filteredNotes =await _dbContext.Notes
            .Where(note => note.UserId == userId && note.Tags.HasValue && note.Tags.Value == (Tag)enumTagValue) // Фильтруем на основе nullable Tag
            .Select(note => new NoteVm(note.UserId, note.Id, note.Name, note.Description, note.Tags)) // Преобразуем в NoteVm
            .ToListAsync(); // Выполняем запрос и получаем список

        return filteredNotes;
    }

    public async Task<List<NoteVm>> SortedByTags(Enum tags, int userId)
    {
        var enumTagValue = Convert.ToByte(tags); 

        var sortedNotes =await _dbContext.Notes
            .Where(note => note.UserId == userId) 
            .OrderByDescending(note => note.Tags.HasValue && note.Tags.Value == (Tag)enumTagValue) 
            .ThenBy(note => note.Tags == null ? 1 : 0) 
            .ThenBy(note => note.Name) 
            .Select(note => new NoteVm(note.UserId, note.Id, note.Name, note.Description, note.Tags)) 
            .ToListAsync(); 

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