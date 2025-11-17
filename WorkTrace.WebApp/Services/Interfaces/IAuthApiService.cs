using WorkTrace.WebApp.Models.Dtos.Users;

namespace WorkTrace.WebApp.Services.Interfaces;

public interface IAuthApiService
{
    Task<LoginResponse?> LoginAsync(LoginRequest request);
}