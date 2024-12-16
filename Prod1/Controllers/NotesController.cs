using Microsoft.AspNetCore.Mvc;
using btlz.Abstractions;
using btlz.Contracts;
using btlz.Models;
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
    [Authorize(Policy = "NotesOwner")]
    public ActionResult<Note> GetNotes(int userId) 
        => Ok(_notesRepository.GetNotes(userId));
    
    [HttpGet("/FilteredByTags")]
    [Authorize(Policy = "NotesOwner")]
    public ActionResult<Note> FilteredByTags(Tag tag, int userId) 
        => Ok(_notesRepository.FilteredByTags(tag, userId));
    
    [HttpGet("/SortedByTags")]
    [Authorize(Policy = "NotesOwner")]
    public ActionResult<Note> SortedByTags(Tag tag, int userId) 
        => Ok(_notesRepository.SortedByTags(tag, userId));
    
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

