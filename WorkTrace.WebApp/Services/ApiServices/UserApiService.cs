using System.Net.Http.Json;
using WorkTrace.WebApp.Models.Dtos.Users;
using WorkTrace.WebApp.Services.Interfaces;

namespace WorkTrace.WebApp.Services.ApiServices;

public class UserApiService : BaseApiService, IUserApiService
{
    private readonly string _controller = "User";

    public UserApiService(HttpClient client) : base(client)
    {
    }

    public async Task<List<UserInformationResponse>?> GetAllAsync()
    {
        var response = await _client.GetAsync($"{_controller}/GetAll");
        return await ReadResponse<List<UserInformationResponse>>(response);
    }

    public async Task<UserInformationResponse?> GetByIdAsync(string id)
    {
        var response = await _client.GetAsync($"{_controller}/GetById?id={id}");
        return await ReadResponse<UserInformationResponse>(response);
    }

    public async Task<UserInformationResponse?> CreateAsync(CreateUserRequest request)
    {
        var response = await _client.PostAsJsonAsync($"{_controller}/Create", request);
        return await ReadResponse<UserInformationResponse>(response);
    }

    public async Task<UserInformationResponse?> UpdateAsync(string id, UpdateUserRequest request)
    {
        var response = await _client.PutAsJsonAsync($"{_controller}/Update/{id}", request);
        return await ReadResponse<UserInformationResponse>(response);
    }

    public async Task<(bool, string?)> DeactivateAsync(string id)
    {
        var response = await _client.PutAsync($"{_controller}/DeactivateUser/{id}/deactivate", null);
        if (response.IsSuccessStatusCode)
        {
            return (true, null);
        }

        var errorContent = await response.Content.ReadAsStringAsync();
        return (false, errorContent);
    }
}
