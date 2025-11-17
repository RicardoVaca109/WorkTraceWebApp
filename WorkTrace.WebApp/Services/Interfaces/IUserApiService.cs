using WorkTrace.WebApp.Models.Dtos.Users;

namespace WorkTrace.WebApp.Services.Interfaces;

public interface IUserApiService
{
    Task<List<UserInformationResponse>?> GetAllAsync();
    Task<UserInformationResponse?> GetByIdAsync(string id);
    Task<UserInformationResponse?> CreateAsync(CreateUserRequest request);
    Task<UserInformationResponse?> UpdateAsync(string id, UpdateUserRequest request);
    Task<bool> DeactivateAsync(string id);
}