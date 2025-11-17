using WorkTrace.WebApp.Models.Dtos.Users;
using WorkTrace.WebApp.Services.Interfaces;

namespace WorkTrace.WebApp.Services;

public class UserApiService : BaseApiService, IUserApiService
{
    private readonly string _controller = "User";

    public UserApiService(
        IHttpClientFactory factory,
        IConfiguration config,
        IHttpContextAccessor contextAccessor
    ) : base(factory, config, contextAccessor)
    {
    }

    public async Task<List<UserInformationResponse>> GetAllAsync()
    {
        var client = CreateHttpClient();
        var response = await client.GetAsync($"{_controller}/GetAll");
        return await ReadResponse<List<UserInformationResponse>>(response);
    }

    public async Task<UserInformationResponse> GetByIdAsync(string id)
    {
        var client = CreateHttpClient();
        var response = await client.GetAsync($"{_controller}/GetById?id={id}");
        return await ReadResponse<UserInformationResponse>(response);
    }

    public async Task<UserInformationResponse> CreateAsync(CreateUserRequest request)
    {
        var client = CreateHttpClient();
        var response = await client.PostAsJsonAsync($"{_controller}/Create", request);
        return await ReadResponse<UserInformationResponse>(response);
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        var client = CreateHttpClient();
        var response = await client.PostAsJsonAsync($"{_controller}/Login", request);
        return await ReadResponse<LoginResponse>(response);
    }

    public async Task<UserInformationResponse> UpdateAsync(string id, UpdateUserRequest request)
    {
        var client = CreateHttpClient();
        var response = await client.PutAsJsonAsync($"{_controller}/Update/{id}", request);
        return await ReadResponse<UserInformationResponse>(response);
    }

    public async Task<bool> DeactivateAsync(string id)
    {
        var client = CreateHttpClient();
        var response = await client.PutAsync($"{_controller}/DeactivateUser/{id}", null!);
        var result = await ReadResponse<string>(response);
        return result != null;
    }
}
