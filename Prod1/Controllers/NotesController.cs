using Microsoft.AspNetCore.Mvc;
using btlz.Abstractions;
using btlz.Contracts;
using AutoMapper;
using btlz.Models;

namespace btlz.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotesController : BaseController
{
    private readonly INotesRepository _notesRepository;
    private readonly IUserRepository _userRepository;
    
    public NotesController(INotesRepository notesRepository, IMapper mapper) : base(mapper)
        => _notesRepository = notesRepository;

    [HttpGet]
    public ActionResult<ListOfNotes> GetNotes()
    {
        var notes = _notesRepository.GetNotes();
        return Ok(Mapper.Map<ListOfNotes>(notes));
    }

    [HttpGet("{userId}")]
    public ActionResult<ListOfNotes> GetNotesByUserId(int userId)
    {
        var notes = _notesRepository.GetNotesByUserId(userId);
        return Ok(Mapper.Map<ListOfNotes>(notes));;
    }

    [HttpPost]
    public ActionResult<int> AddNotes(CreateNotesDto dto)
    {
        var newNotes = Mapper.Map<Note>(dto);
        var userId = dto.UserId;  
        var notesId = _notesRepository.AddNotes(newNotes, userId);
        return Ok(notesId);
    }
    
    [HttpPut("{id}")]
    public ActionResult UpdateNotes(int id, UpdateNotesDto dto)
    {
        var updatedNotes = Mapper.Map<Note>((id, dto));
        _notesRepository.UpdateNotes(id, updatedNotes);
        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public ActionResult DeleteNotes(int id)
    {
        _notesRepository.DeleteNotes(id);
        return NoContent();
    }
    
}