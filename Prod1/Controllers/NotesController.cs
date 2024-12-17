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
    public async Task<ActionResult<Note>> GetNotes(int userId) 
        => Ok(await _notesRepository.GetNotes(userId));
    
    [HttpGet("/FilteredByTags")]
    [Authorize(Policy = "NotesOwner")]
    public async Task<ActionResult<Note>> FilteredByTags(Tag tag, int userId) 
        => Ok(await _notesRepository.FilteredByTags(tag, userId));
    
    [HttpGet("/SortedByTags")]
    [Authorize(Policy = "NotesOwner")]
    public async Task<ActionResult<Note>> SortedByTags(Tag tag, int userId) 
        => Ok(await _notesRepository.SortedByTags(tag, userId));
    
    [HttpPost]
    public async Task<ActionResult<Note>> AddNotes(CreateNotesDto dto)
    {
        Note note = new()
        {
             Name = dto.Name,
             Description = dto.Description,
             UserId = HttpContext.ExtractUserIdFromClaims()!.Value,
             Finished = false,
             DateCreated = DateTime.UtcNow
        };
        await _notesRepository.AddNotes(note);
        return  Ok(note);
    }
    
    [HttpPut("{id}")]
    [Authorize(Policy = "NotesOwner")]
    public async Task<ActionResult<int>> UpdateNotes(int userId, int id, UpdateNotesDto dto)
        => Ok(await _notesRepository.UpdateNotes(id, dto));
    
    [HttpDelete("{id}")]
    [Authorize(Policy = "NotesOwner")]
    public async Task<ActionResult> DeleteNotes(int userId, int id)
    {
        await _notesRepository.DeleteNotes(id);
        return NoContent();
    }
}

