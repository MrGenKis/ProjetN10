using System.Net.Http.Json;
using RiskService.Models;

namespace RiskService.Services;

public class NoteApiService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public NoteApiService(
        HttpClient httpClient,
        IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<List<NoteDto>> GetNotesByPatientAsync(
        int patientId)
    {
        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"api/notes/patient/{patientId}"
        );

        AddAuthorizationHeader(request);

        var response =
            await _httpClient.SendAsync(request);

        response.EnsureSuccessStatusCode();

        var notes =
            await response.Content
                .ReadFromJsonAsync<List<NoteDto>>();

        return notes ?? new List<NoteDto>();
    }

    private void AddAuthorizationHeader(
        HttpRequestMessage request)
    {
        var authorizationHeader =
            _httpContextAccessor
                .HttpContext?
                .Request
                .Headers["Authorization"]
                .ToString();

        if (!string.IsNullOrWhiteSpace(authorizationHeader))
        {
            request.Headers.TryAddWithoutValidation(
                "Authorization",
                authorizationHeader
            );
        }
    }
}