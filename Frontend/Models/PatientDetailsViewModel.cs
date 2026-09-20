namespace Frontend.Models;

public class PatientDetailsViewModel
{
    public PatientViewModel Patient { get; set; } = new();

    public List<NoteViewModel> Notes { get; set; } = new();

    public RiskAssessmentViewModel? RiskAssessment { get; set; }

    public string NewNoteContent { get; set; } = string.Empty;
}