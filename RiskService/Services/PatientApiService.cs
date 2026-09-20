using System.Net.Http.Json;
using RiskService.Models;

namespace RiskService.Services;

public class PatientApiService
{
    private readonly HttpClient _httpClient;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PatientApiService(
        HttpClient httpClient,
        IHttpContextAccessor httpContextAccessor)
    {
        _httpClient = httpClient;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<PatientDto?> GetPatientAsync(int patientId)
    {
        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"api/patients/{patientId}"
        );

        AddAuthorizationHeader(request);

        var response =
            await _httpClient.SendAsync(request);

        if (response.StatusCode ==
            System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        return await response.Content
            .ReadFromJsonAsync<PatientDto>();
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