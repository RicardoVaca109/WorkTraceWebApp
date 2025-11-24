using WorkTrace.WebApp.Models.Dtos.Status;

namespace WorkTrace.WebApp.Services.Interfaces
{
    public interface IStatusApiService
    {
        Task<List<StatusInformationResponse>?> GetAllAsync();
        Task<StatusInformationResponse?> CreateAsync(CreateStatusRequest request);
    }
}
