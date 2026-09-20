using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoteService.Data;
using NoteService.Models;

namespace NoteService.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class NotesController : ControllerBase
{
    private readonly NoteDbContext _context;

    public NotesController(NoteDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Note>>> GetNotes()
    {
        var notes =
            await _context.Notes
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

        return Ok(notes);
    }

    [HttpGet("patient/{patientId}")]
    public async Task<ActionResult<IEnumerable<Note>>> GetNotesByPatient(
        int patientId)
    {
        var notes =
            await _context.Notes
                .Where(n => n.PatientId == patientId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

        return Ok(notes);
    }

    [HttpPost]
    public async Task<ActionResult<Note>> CreateNote(Note note)
    {
        if (note.PatientId <= 0)
        {
            return BadRequest(
                "L'identifiant du patient est invalide."
            );
        }

        if (string.IsNullOrWhiteSpace(note.Content))
        {
            return BadRequest(
                "Le contenu de la note est obligatoire."
            );
        }

        note.Id = Guid.NewGuid().ToString();
        note.CreatedAt = DateTime.UtcNow;

        _context.Notes.Add(note);

        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetNotesByPatient),
            new { patientId = note.PatientId },
            note
        );
    }
}