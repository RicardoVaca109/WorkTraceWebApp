using WorkTrace.WebApp.Models.Dtos.FormTemplate;

namespace WorkTrace.WebApp.Services.Interfaces
{
    public interface IFormTemplateApiService
    {
        Task<List<FormTemplateResponse>?> GetAllAsync();
        Task<FormTemplateResponse?> GetByIdAsync(string id);
        Task<FormTemplateResponse?> CreateAsync(CreateFormTemplateRequest request);
        Task<FormTemplateResponse?> UpdateAsync(string id, UpdateFormTemplateRequest request);
        Task<FormTemplateResponse?> UpdateQuestionsAsync(string id, List<UpdateFormQuestionsRequest> questions);
        Task<(bool, string?)> ActivateAsync(string id);
        Task<(bool, string?)> DeactivateAsync(string id);
    }
}
