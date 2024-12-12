using System.Collections.Concurrent;
using Microsoft.AspNetCore.Mvc;
using btlz.Abstractions;
using btlz.Contracts;
using btlz.Models;
using Microsoft.AspNetCore.Authorization;

namespace btlz.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NotesController : BaseController
{
    private static readonly ConcurrentBag<Note> Notes = new();

    [HttpPost]
    public IActionResult AddNote(string name)
    {
        Note note = new()
        {
            Id = Notes.Count,
            UserId = HttpContext.ExtractUserIdFromClaims()!.Value, 
            Name = name,
            Finished = false,
        };
        Notes.Add(note);
        return Ok();
    }

    [HttpGet]
    public List<Note> GetNotes() 
        => Notes
            .Where(note => 
                note.UserId == HttpContext.ExtractUserIdFromClaims()!.Value)
            .ToList();

    [HttpPut]
    [Authorize]
    public IActionResult Complete(int noteId, int userId)
    {
        var note = Notes.FirstOrDefault(note => 
            note.Id == noteId 
            && note.UserId == userId);
        if (note is null)
        {
            return NotFound();
        }
        note.Finished = true;
        return Ok();
    }
    
    [HttpDelete("{id}")]
    public ActionResult DeleteNotes(int id)
    {
        var note = Notes.FirstOrDefault(note =>
            note.Id == id);
        if (note is null)
        {
            return NotFound();
        }

        return Ok();
    }
}