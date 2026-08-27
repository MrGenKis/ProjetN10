using System.Net.Http.Json;
using Frontend.Models;

namespace Frontend.Services;

public class NoteApiService
{
    private readonly HttpClient _httpClient;

    public NoteApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<NoteViewModel>> GetNotesByPatientAsync(int patientId)
    {
        var notes = await _httpClient
            .GetFromJsonAsync<List<NoteViewModel>>(
                $"api/notes/patient/{patientId}"
            );

        return notes ?? new List<NoteViewModel>();
    }

    public async Task<bool> CreateNoteAsync(int patientId, string content)
    {
        var note = new NoteViewModel
        {
            PatientId = patientId,
            Content = content
        };

        var response = await _httpClient.PostAsJsonAsync(
            "api/notes",
            note
        );

        return response.IsSuccessStatusCode;
    }
}