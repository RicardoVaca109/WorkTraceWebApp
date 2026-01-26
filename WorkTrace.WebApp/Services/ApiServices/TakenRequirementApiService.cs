using WorkTrace.WebApp.Models.Dtos.TakenRequirement;
using WorkTrace.WebApp.Services.Interfaces;

namespace WorkTrace.WebApp.Services.ApiServices;

public class TakenRequirementApiService : BaseApiService, ITakenRequirementApiService
{
    private readonly string _controller = "TakenRequirements";

    public TakenRequirementApiService(HttpClient client) : base(client)
    {
    }

    public async Task<List<TakenRequirementResponse>?> GetAllAsync()
    {
        var response = await _client.GetAsync($"{_controller}/GetAll");
        return await ReadResponse<List<TakenRequirementResponse>>(response);
    }

    public async Task<TakenRequirementResponse?> CreateAsync(CreateTakenRequirementRequest request)
    {
        var response = await _client.PostAsJsonAsync($"{_controller}/Create", request);
        return await ReadResponse<TakenRequirementResponse>(response);
    }

    public async Task<TakenRequirementResponse?> UpdateAsync(string id, UpdateTakenRequirementRequest request)
    {
        var response = await _client.PutAsJsonAsync($"{_controller}/Update/{id}", request);
        return await ReadResponse<TakenRequirementResponse>(response);
    }

    public async Task<List<TakenRequirementUserAndClientResponse>?> GetByDateAllAsync(DateTime start, DateTime end)
    {
        var response = await _client.GetAsync($"{_controller}/GetByDateAll?start={start:yyyy-MM-dd}&end={end:yyyy-MM-dd}");
        return await ReadResponse<List<TakenRequirementUserAndClientResponse>>(response);
    }
}
