using Microsoft.AspNetCore.Mvc;
using RiskService.Services;

namespace RiskService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RiskController : ControllerBase
{
    private readonly RiskAssessmentService _riskAssessmentService;

    public RiskController(
        RiskAssessmentService riskAssessmentService)
    {
        _riskAssessmentService = riskAssessmentService;
    }

    // GET: api/risk/1
    [HttpGet("{patientId}")]
    public async Task<IActionResult> GetRisk(int patientId)
    {
        var assessment =
            await _riskAssessmentService.AssessRiskAsync(patientId);

        if (assessment == null)
        {
            return NotFound(
                $"Le patient avec l'identifiant {patientId} n'existe pas."
            );
        }

        return Ok(assessment);
    }
}