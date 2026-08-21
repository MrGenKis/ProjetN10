using Frontend.Models;
using Frontend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Frontend.Controllers;

public class PatientsController : Controller
{
    private readonly PatientApiService _patientApiService;

    public PatientsController(PatientApiService patientApiService)
    {
        _patientApiService = patientApiService;
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

        return View(patient);
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

        var success = await _patientApiService.CreatePatientAsync(patient);

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
        var patient = await _patientApiService.GetPatientAsync(id);

        if (patient == null)
        {
            return NotFound();
        }

        return View(patient);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(PatientViewModel patient)
    {
        if (!ModelState.IsValid)
        {
            return View(patient);
        }

        var success = await _patientApiService.UpdatePatientAsync(patient);

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