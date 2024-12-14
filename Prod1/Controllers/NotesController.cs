using Microsoft.AspNetCore.Mvc;
using btlz.Abstractions;
using btlz.Contracts;
using btlz.Models;
using btlz.Services;
using Microsoft.AspNetCore.Authorization;

namespace btlz.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]

public class NotesController : BaseController
{
    private readonly INotesRepository _notesRepository;
    
    public NotesController(INotesRepository notesRepository) 
        => _notesRepository = notesRepository;

    [HttpGet]
    public ActionResult<Note> GetNotes()
    {
        List<NoteVm> notes = _notesRepository.GetNotes();
       var listOfNotes = notes.Where((note =>
            note.UserId == HttpContext.ExtractUserIdFromClaims()!.Value)).ToList();
        return Ok(listOfNotes);
    }
    
    [HttpPost]
    public ActionResult<Note> AddNotes(CreateNotesDto dto)
    {
        Note note = new()
        {
             Name = dto.Name,
             Description = dto.Description,
             UserId = HttpContext.ExtractUserIdFromClaims()!.Value,
             Finished = false,
             DateCreated = DateTime.UtcNow
        };
        _notesRepository.AddNotes(note);
        return Ok(note);
    }


    [HttpPut("{id}")]
    [Authorize(Policy = "NotesOwner")]
    public ActionResult<int> UpdateNotes(int userId, int id, UpdateNotesDto dto)
        => Ok(_notesRepository.UpdateNotes(id, dto));
    
    [HttpDelete("{id}")]
    [Authorize(Policy = "NotesOwner")]
    public ActionResult DeleteNotes(int userId, int id)
    {
        _notesRepository.DeleteNotes(id);
        return NoContent();
    }
}

