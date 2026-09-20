using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PatientService.Data;
using PatientService.Models;

namespace PatientService.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public PatientsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Patient>>> GetPatients()
    {
        var patients =
            await _context.Patients.ToListAsync();

        return Ok(patients);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Patient>> GetPatient(int id)
    {
        var patient =
            await _context.Patients.FindAsync(id);

        if (patient == null)
        {
            return NotFound();
        }

        return Ok(patient);
    }

    [HttpPost]
    public async Task<ActionResult<Patient>> CreatePatient(
        Patient patient)
    {
        _context.Patients.Add(patient);

        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetPatient),
            new { id = patient.Id },
            patient
        );
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePatient(
        int id,
        Patient patient)
    {
        if (id != patient.Id)
        {
            return BadRequest();
        }

        var existingPatient =
            await _context.Patients.FindAsync(id);

        if (existingPatient == null)
        {
            return NotFound();
        }

        existingPatient.FirstName =
            patient.FirstName;

        existingPatient.LastName =
            patient.LastName;

        existingPatient.DateOfBirth =
            patient.DateOfBirth;

        existingPatient.Gender =
            patient.Gender;

        existingPatient.Address =
            patient.Address;

        existingPatient.PhoneNumber =
            patient.PhoneNumber;

        await _context.SaveChangesAsync();

        return NoContent();
    }
}