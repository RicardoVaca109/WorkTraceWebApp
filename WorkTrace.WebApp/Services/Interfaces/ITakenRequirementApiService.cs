using WorkTrace.WebApp.Models.Dtos.TakenRequirement;

namespace WorkTrace.WebApp.Services.Interfaces;

public interface ITakenRequirementApiService
{
    Task<List<TakenRequirementResponse>?> GetAllAsync();
    Task<TakenRequirementResponse?> CreateAsync(CreateTakenRequirementRequest request);
    Task<TakenRequirementResponse?> UpdateAsync(string id, UpdateTakenRequirementRequest request);
}
