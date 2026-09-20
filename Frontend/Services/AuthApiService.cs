using System.Net.Http.Json;
using Frontend.Models;

namespace Frontend.Services;

public class AuthApiService
{
    private readonly HttpClient _httpClient;

    public AuthApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<LoginResponseViewModel?> LoginAsync(
        string email,
        string password)
    {
        var request = new LoginViewModel
        {
            Email = email,
            Password = password
        };

        var response = await _httpClient.PostAsJsonAsync(
            "api/auth/login",
            request
        );

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        return await response.Content
            .ReadFromJsonAsync<LoginResponseViewModel>();
    }
}