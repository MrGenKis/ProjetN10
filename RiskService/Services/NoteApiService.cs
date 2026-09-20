using System.Net.Http.Json;
using RiskService.Models;

namespace RiskService.Services;

public class NoteApiService
{
    private readonly HttpClient _httpClient;

    public NoteApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<NoteDto>> GetNotesByPatientAsync(int patientId)
    {
        var notes = await _httpClient
            .GetFromJsonAsync<List<NoteDto>>(
                $"api/notes/patient/{patientId}"
            );

        return notes ?? new List<NoteDto>();
    }
}