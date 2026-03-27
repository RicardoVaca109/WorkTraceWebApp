using WorkTrace.WebApp.Models.Dtos.Clients;

namespace WorkTrace.WebApp.Services.Interfaces
{
    public interface IClientApiService
    {
        Task<List<ClientInformationResponse>?> GetAllAsync();
        Task<ClientInformationResponse?> CreateAsync(CreateClientRequest request);
        Task<ClientInformationResponse?> UpdateAsync(string id, UpdateClientRequest request);
        Task<ClientInformationResponse?> GetByIdAsync(string id);
    }
}
