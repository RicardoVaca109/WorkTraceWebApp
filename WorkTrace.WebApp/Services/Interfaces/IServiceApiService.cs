using WorkTrace.WebApp.Models.Dtos.Service;
using System.Threading.Tasks;

namespace WorkTrace.WebApp.Services.Interfaces
{
    public interface IServiceApiService
    {
        Task<List<ServiceInformationResponse>?> GetAllAsync();
        Task<ServiceInformationResponse?> CreateAsync(CreateServiceRequest request);
        Task<ServiceInformationResponse?> GetByIdAsync(string id);
        Task<ServiceInformationResponse?> UpdateAsync(string id, UpdateServiceRequest request);
    }
}
