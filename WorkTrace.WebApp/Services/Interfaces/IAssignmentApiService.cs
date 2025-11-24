using System.Collections.Generic;
using System.Threading.Tasks;
using WorkTrace.WebApp.Models.Dtos;

namespace WorkTrace.WebApp.Services.Interfaces
{
    public interface IAssignmentApiService
    {
        Task<List<AssignmentResponse>?> GetAllAsync();
        Task<List<ClientHistoryResponse>?> GetClientHistoryAsync(string clientId);
        Task<AssignmentResponse?> CreateAsync(CreateAssignmentRequest request);
        Task<AssignmentResponse?> UpdateAsync(string id, UpdateAssignmentRequest request);
        Task<AssignmentResponse?> GetByIdAsync(string id);
    }
}
