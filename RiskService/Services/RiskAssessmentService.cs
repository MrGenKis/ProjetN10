using RiskService.Models;

namespace RiskService.Services;

public class RiskAssessmentService
{
    private readonly PatientApiService _patientApiService;
    private readonly NoteApiService _noteApiService;

    private readonly string[] _triggerTerms =
    {
        "Hémoglobine A1C",
        "Microalbumine",
        "Taille",
        "Poids",
        "Fumeur",
        "Fumeuse",
        "Anormal",
        "Cholestérol",
        "Vertiges",
        "Vertige",
        "Rechute",
        "Réaction",
        "Anticorps"
    };

    public RiskAssessmentService(
        PatientApiService patientApiService,
        NoteApiService noteApiService)
    {
        _patientApiService = patientApiService;
        _noteApiService = noteApiService;
    }

    public async Task<RiskAssessmentDto?> AssessRiskAsync(int patientId)
    {
        var patient = await _patientApiService.GetPatientAsync(patientId);

        if (patient == null)
        {
            return null;
        }

        var notes = await _noteApiService.GetNotesByPatientAsync(patientId);

        var age = CalculateAge(patient.DateOfBirth);

        var triggerCount = CountTriggers(notes);

        var riskLevel = DetermineRiskLevel(
            age,
            patient.Gender,
            triggerCount
        );

        return new RiskAssessmentDto
        {
            PatientId = patient.Id,
            PatientName = $"{patient.FirstName} {patient.LastName}",
            Age = age,
            TriggerCount = triggerCount,
            RiskLevel = riskLevel
        };
    }

    private int CalculateAge(DateTime dateOfBirth)
    {
        var today = DateTime.Today;

        var age = today.Year - dateOfBirth.Year;

        if (dateOfBirth.Date > today.AddYears(-age))
        {
            age--;
        }

        return age;
    }

    private int CountTriggers(List<NoteDto> notes)
    {
        var allNotes = string.Join(
            " ",
            notes.Select(n => n.Content)
        );

        var count = 0;

        foreach (var trigger in _triggerTerms)
        {
            if (allNotes.Contains(
                trigger,
                StringComparison.OrdinalIgnoreCase))
            {
                count++;
            }
        }

        return count;
    }

    private string DetermineRiskLevel(
        int age,
        string gender,
        int triggerCount)
    {
        if (triggerCount == 0)
        {
            return "None";
        }

        if (age > 30)
        {
            if (triggerCount >= 8)
            {
                return "Early onset";
            }

            if (triggerCount >= 6)
            {
                return "In Danger";
            }

            if (triggerCount >= 2)
            {
                return "Borderline";
            }

            return "None";
        }

        var isMale =
            gender.Equals("M", StringComparison.OrdinalIgnoreCase);

        if (isMale)
        {
            if (triggerCount >= 5)
            {
                return "Early onset";
            }

            if (triggerCount >= 3)
            {
                return "In Danger";
            }
        }
        else
        {
            if (triggerCount >= 7)
            {
                return "Early onset";
            }

            if (triggerCount >= 4)
            {
                return "In Danger";
            }
        }

        return "None";
    }
}