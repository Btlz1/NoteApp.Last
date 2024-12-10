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
    
    public NotesController(INotesRepository notesRepository) 
        => _notesRepository = notesRepository;

    [HttpGet]
    public ActionResult<NotesVm> GetNotes()
        => Ok(_notesRepository.GetNotes());
    
    [HttpGet("{userId}")]
    public ActionResult<NotesVm> GetNotesByUserId(int userId)
        => Ok(_notesRepository.GetNotesByUserId(userId));
    
    [HttpPost]
    public ActionResult<NoteVm> AddNotes(int userId, CreateNotesDto dto)
        => Ok(_notesRepository.AddNotes(userId, dto));
    
    [HttpPut("{id}")]
    public ActionResult UpdateNotes(int id, UpdateNotesDto dto)
        => Ok(_notesRepository.UpdateNotes(id, dto));
    
    [HttpDelete("{id}")]
    public ActionResult DeleteNotes(int id)
    {
        _notesRepository.DeleteNotes(id);
        return NoContent();
    }
    
}