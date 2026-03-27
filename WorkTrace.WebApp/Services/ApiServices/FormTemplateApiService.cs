using WorkTrace.WebApp.Models.Dtos.FormTemplate;
using WorkTrace.WebApp.Services.Interfaces;

namespace WorkTrace.WebApp.Services.ApiServices
{
    public class FormTemplateApiService : BaseApiService, IFormTemplateApiService
    {
        private readonly string _controller = "FormTemplate";

        public FormTemplateApiService(HttpClient client) : base(client)
        {
        }

        public async Task<List<FormTemplateResponse>?> GetAllAsync()
        {
            var response = await _client.GetAsync($"{_controller}/GetAll");
            return await ReadResponse<List<FormTemplateResponse>>(response);
        }

        public async Task<FormTemplateResponse?> GetByIdAsync(string id)
        {
            var response = await _client.GetAsync($"{_controller}/GetById/{id}");
            return await ReadResponse<FormTemplateResponse>(response);
        }

        public async Task<FormTemplateResponse?> CreateAsync(CreateFormTemplateRequest request)
        {
            var response = await _client.PostAsJsonAsync($"{_controller}/Create", request);
            return await ReadResponse<FormTemplateResponse>(response);
        }

        public async Task<FormTemplateResponse?> UpdateAsync(string id, UpdateFormTemplateRequest request)
        {
            var response = await _client.PutAsJsonAsync($"{_controller}/Update/{id}", request);
            return await ReadResponse<FormTemplateResponse>(response);
        }

        public async Task<FormTemplateResponse?> UpdateQuestionsAsync(string id, List<UpdateFormQuestionsRequest> questions)
        {
            var response = await _client.PutAsJsonAsync($"{_controller}/UpdateQuestions/{id}/questions", questions);
            return await ReadResponse<FormTemplateResponse>(response);
        }

        public async Task<(bool, string?)> ActivateAsync(string id)
        {
            var response = await _client.PatchAsync($"{_controller}/Activate/{id}/activate", null);
            if (response.IsSuccessStatusCode) return (true, null);
            return (false, await response.Content.ReadAsStringAsync());
        }

        public async Task<(bool, string?)> DeactivateAsync(string id)
        {
            var response = await _client.PatchAsync($"{_controller}/Deactivate/{id}/deactivate", null);
            if (response.IsSuccessStatusCode) return (true, null);
            return (false, await response.Content.ReadAsStringAsync());
        }
    }
}
