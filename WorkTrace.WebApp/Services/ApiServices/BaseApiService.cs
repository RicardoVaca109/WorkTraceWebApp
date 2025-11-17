using System.Net.Http.Headers;
using System.Text.Json;

namespace WorkTrace.WebApp.Services;

public abstract class BaseApiService
{
    private readonly IHttpClientFactory _factory;
    private readonly IConfiguration _config;
    private readonly IHttpContextAccessor _contextAccessor;

    protected BaseApiService(IHttpClientFactory factory, IConfiguration config, IHttpContextAccessor contextAccessor)
    {
        _factory = factory;
        _config = config;
        _contextAccessor = contextAccessor;
    }

    protected HttpClient CreateHttpClient()
    {
        var client = _factory.CreateClient();

        string? baseUrl = _config["APIConfigurations:ApiUrl"];
        client.BaseAddress = new Uri(baseUrl);

        // Leer token si lo tienes guardado en sesión
        var token = _contextAccessor.HttpContext?.Session.GetString("AuthToken");

        if (!string.IsNullOrEmpty(token))
        {
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }

        return client;
    }

    protected async Task<T?> ReadResponse<T>(HttpResponseMessage response)
    {
        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(
            content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}