using System.Net.Http.Json;
using Frontend.Models;

namespace Frontend.Services;

public class PatientApiService
{
    private readonly HttpClient _httpClient;

    public PatientApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<PatientViewModel>> GetPatientsAsync()
    {
        var patients = await _httpClient
            .GetFromJsonAsync<List<PatientViewModel>>("api/patients");

        return patients ?? new List<PatientViewModel>();
    }

    public async Task<PatientViewModel?> GetPatientAsync(int id)
    {
        return await _httpClient
            .GetFromJsonAsync<PatientViewModel>($"api/patients/{id}");
    }

    public async Task<bool> CreatePatientAsync(PatientViewModel patient)
    {
        var response = await _httpClient.PostAsJsonAsync(
            "api/patients",
            patient
        );

        return response.IsSuccessStatusCode;
    }

    public async Task<bool> UpdatePatientAsync(PatientViewModel patient)
    {
        var response = await _httpClient.PutAsJsonAsync(
            $"api/patients/{patient.Id}",
            patient
        );

        return response.IsSuccessStatusCode;
    }
}