using Frontend.Models;
using Frontend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers;

public class PatientsController : Controller
{
    private readonly PatientApiService _patientApiService;
    private readonly NoteApiService _noteApiService;
    private readonly RiskApiService _riskApiService;

    public PatientsController(
        PatientApiService patientApiService,
        NoteApiService noteApiService,
        RiskApiService riskApiService)
    {
        _patientApiService = patientApiService;
        _noteApiService = noteApiService;
        _riskApiService = riskApiService;
    }

    public async Task<IActionResult> Index()
    {
        var patients = await _patientApiService.GetPatientsAsync();

        return View(patients);
    }

    public async Task<IActionResult> Details(int id)
    {
        var patient = await _patientApiService.GetPatientAsync(id);

        if (patient == null)
        {
            return NotFound();
        }

        var notes = await _noteApiService.GetNotesByPatientAsync(id);

        var riskAssessment = await _riskApiService.GetRiskAsync(id);

        var viewModel = new PatientDetailsViewModel
        {
            Patient = patient,
            Notes = notes,
            RiskAssessment = riskAssessment
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddNote(
        int patientId,
        string newNoteContent)
    {
        if (patientId <= 0)
        {
            return BadRequest();
        }

        if (string.IsNullOrWhiteSpace(newNoteContent))
        {
            TempData["NoteError"] = "La note ne peut pas être vide.";

            return RedirectToAction(
                nameof(Details),
                new { id = patientId }
            );
        }

        var success = await _noteApiService.CreateNoteAsync(
            patientId,
            newNoteContent
        );

        if (!success)
        {
            TempData["NoteError"] =
                "Impossible d'ajouter la note.";
        }

        return RedirectToAction(
            nameof(Details),
            new { id = patientId }
        );
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PatientViewModel patient)
    {
        if (!ModelState.IsValid)
        {
            return View(patient);
        }

        var success =
            await _patientApiService.CreatePatientAsync(patient);

        if (!success)
        {
            ModelState.AddModelError(
                string.Empty,
                "Une erreur est survenue lors de la création du patient."
            );

            return View(patient);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var patient =
            await _patientApiService.GetPatientAsync(id);

        if (patient == null)
        {
            return NotFound();
        }

        return View(patient);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        PatientViewModel patient)
    {
        if (!ModelState.IsValid)
        {
            return View(patient);
        }

        var success =
            await _patientApiService.UpdatePatientAsync(patient);

        if (!success)
        {
            ModelState.AddModelError(
                string.Empty,
                "Une erreur est survenue lors de la modification du patient."
            );

            return View(patient);
        }

        return RedirectToAction(nameof(Index));
    }
}