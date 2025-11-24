using System.Text;
using System.Text.Json;
using WorkTrace.WebApp.Models.Dtos.Users;
using WorkTrace.WebApp.Services.Interfaces;

namespace WorkTrace.WebApp.Services.ApiServices
{
    public class AuthApiService : IAuthApiService
    {
        private readonly HttpClient _client;

        public AuthApiService(HttpClient client)
        {
            _client = client;
        }

        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            var response = await _client.PostAsJsonAsync("User/Login", request);

            if (!response.IsSuccessStatusCode)
                return null;

            var responseJson = await response.Content.ReadAsStringAsync();
            var loginResponse = JsonSerializer.Deserialize<LoginResponse>(responseJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return loginResponse;
        }
    }
}