using System.Net.Http.Json;
using RiskService.Models;

namespace RiskService.Services;

public class PatientApiService
{
    private readonly HttpClient _httpClient;

    public PatientApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PatientDto?> GetPatientAsync(int patientId)
    {
        return await _httpClient
            .GetFromJsonAsync<PatientDto>($"api/patients/{patientId}");
    }
}