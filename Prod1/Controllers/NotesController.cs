using Microsoft.AspNetCore.Mvc;
using btlz.Abstractions;
using btlz.Contracts;
using AutoMapper;
using btlz.Models;
using Microsoft.JSInterop.Infrastructure;

namespace btlz.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotesController : Controller
{
    private readonly INotesRepository _notesRepository;
    private readonly IMapper _mapper;
    
    public NotesController(INotesRepository notesRepository, IMapper mapper) => 
        (_notesRepository, _mapper) = (notesRepository, mapper);

    [HttpGet]
    public ActionResult<ListOfNotes> GetNotes()
    {
        var notes = _notesRepository.GetNotes();

        return Ok(_mapper.Map<ListOfNotes>(notes));
    }

    [HttpGet("{userId}")]
    public ActionResult<ListOfNotes> GetNotesByUserId(int userId)
    {
        var notes = _notesRepository.GetNotesByUserId(userId);
        return Ok(_mapper.Map<ListOfNotes>(notes));;
    }

    [HttpPost]
    public ActionResult<int> AddNotes(CreateNotesDto dto)
    {
        var newNotes = _mapper.Map<Note>(dto);
        var userId = dto.UserId;  
        var notesId = _notesRepository.AddNotes(newNotes, userId);
        return Ok(_mapper.Map<ListOfNotes>(newNotes));;
    }
    
    [HttpPut("{id}")]
    public ActionResult UpdateNotes(int id, UpdateNotesDto dto)
    {
        var updatedNotes = _mapper.Map<Note>((id, dto));
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