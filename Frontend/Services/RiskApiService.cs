using System.Net.Http.Json;
using Frontend.Models;

namespace Frontend.Services;

public class RiskApiService
{
    private readonly HttpClient _httpClient;

    public RiskApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<RiskAssessmentViewModel?> GetRiskAsync(int patientId)
    {
        return await _httpClient
            .GetFromJsonAsync<RiskAssessmentViewModel>(
                $"api/risk/{patientId}"
            );
    }
}