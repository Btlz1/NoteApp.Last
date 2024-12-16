using AutoMapper;
using btlz.Abstractions;
using btlz.Contracts;
using btlz.Exceptions;
using btlz.Models;
using btlz.Database;
using System.Linq;


namespace btlz.Services;

public class NotesRepository : INotesRepository 
{
    private readonly btlzDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    public NotesRepository(btlzDbContext dbContext, IMapper mapper, IUserRepository userRepository)
        => (_dbContext, _mapper, _userRepository) = (dbContext, mapper, userRepository);

    public List<NoteVm> GetNotes(int userId)
    {
            var listOfNotes = (
            from note in _dbContext.Notes
            where note.UserId == userId
            select new NoteVm(note.UserId, note.Id, note.Name, note.Description, note.Tags)
        ).ToList();
        return listOfNotes;
    }
    
    public Note AddNotes(Note dto)
    {
        var  note = _mapper.Map<Note>(dto);
        _dbContext.Notes.Add(note);
        _dbContext.SaveChanges();
        return note;
    }

    public int UpdateNotes(int id, UpdateNotesDto dto)
    {
        var note = TryGetNotesByIdAndThrowIfNotFound(id);
        var updatedNote = _mapper.Map<(int, UpdateNotesDto), Note>((id, dto));
        note.Name = updatedNote.Name;
        note.Description = updatedNote.Description;
        note.Finished = true;
        note.EditDate = DateTime.UtcNow;
        note.Tags = updatedNote.Tags;
        _dbContext.SaveChanges();
        return note.Id;
    }
 
    public void DeleteNotes(int id)
    {
        var note = TryGetNotesByIdAndThrowIfNotFound(id);
        _dbContext.Notes.Remove(note);
        _dbContext.SaveChanges();
    }
    
    public List<NoteVm> FilteredByTags(Enum tags, int userId)
    {
        var listOfNotes = (
            from note in _dbContext.Notes
            where note.UserId == userId  
            select new NoteVm(note.UserId, note.Id, note.Name, note.Description, note.Tags)
        ).ToList();
        var filteredList = listOfNotes.Where(n => Equals(n.Tags, tags))
            .ToList();
        return filteredList;
    }

    public List<NoteVm> SortedByTags(Enum tags, int userId)
    {
        var listOfNotes = (
                from note in _dbContext.Notes
                where note.UserId == userId
                select new NoteVm(note.UserId, note.Id, note.Name, note.Description, note.Tags)
            ).ToList();
        var sortedNotes = listOfNotes
            .OrderByDescending(n => Equals(n.Tags, tags))
            .ThenBy(n => n.Name)
            .ToList();
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