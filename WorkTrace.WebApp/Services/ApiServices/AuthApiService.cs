using System.Text;
using System.Text.Json;
using WorkTrace.WebApp.Models.Dtos.Users;
using WorkTrace.WebApp.Services.Interfaces;

namespace WorkTrace.WebApp.Services.ApiServices
{
    public class AuthApiService : IAuthApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public AuthApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        public async Task<LoginResponse?> LoginAsync(LoginRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            var baseUrl = _configuration.GetValue<string>("APIConfigurations:ApiUrl");
            client.BaseAddress = new Uri(baseUrl);

            var json = JsonSerializer.Serialize(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("User/Login", content);

            if (!response.IsSuccessStatusCode)
                return null;

            var responseJson = await response.Content.ReadAsStringAsync();
            var loginResponse = JsonSerializer.Deserialize<LoginResponse>(responseJson,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            return loginResponse;
        }
    }
}