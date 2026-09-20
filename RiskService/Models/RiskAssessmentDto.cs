namespace RiskService.Models;

public class RiskAssessmentDto
{
    public int PatientId { get; set; }

    public string PatientName { get; set; } = string.Empty;

    public int Age { get; set; }

    public int TriggerCount { get; set; }

    public string RiskLevel { get; set; } = string.Empty;
}