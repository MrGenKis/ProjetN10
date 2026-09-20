namespace RiskService.Models;

public class NoteDto
{
    public string Id { get; set; } = string.Empty;

    public int PatientId { get; set; }

    public string Content { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
}