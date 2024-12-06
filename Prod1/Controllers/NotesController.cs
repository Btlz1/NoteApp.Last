using Microsoft.AspNetCore.Mvc;
using btlz.Abstractions;
using btlz.Contracts;
using AutoMapper;
using btlz.Models;

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
        
    
    [HttpPost]
    public ActionResult<int> AddNotes(CreateNotesDto dto)
    {
        var newNotes = _mapper.Map<Notes>(dto);
        var notesId = _notesRepository.AddNotes(newNotes);
        return notesId;
    }
    
    [HttpPut("{id}")]
    public ActionResult UpdateNotes(int id, UpdateNotesDto dto)
    {
        var updatedNotes = _mapper.Map<Notes>((id, dto));
        _notesRepository.UpdateNotes(updatedNotes);
        
        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public ActionResult DeleteNotes(int id)
    {
        _notesRepository.DeleteNotes(id);

        return NoContent();
    }
    
}