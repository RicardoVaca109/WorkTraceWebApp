using WorkTrace.WebApp.Models.Dtos.Service;
using WorkTrace.WebApp.Services.Interfaces;

namespace WorkTrace.WebApp.Services.ApiServices
{
    public class ServiceApiService : BaseApiService, IServiceApiService
    {
        private readonly string _controller = "ServiceandInstallationSteps";

        public ServiceApiService(HttpClient client) : base(client)
        {
        }

        public async Task<List<ServiceInformationResponse>?> GetAllAsync()
        {
            var response = await _client.GetAsync($"{_controller}/GetAll");
            return await ReadResponse<List<ServiceInformationResponse>>(response);
        }

        public async Task<ServiceInformationResponse?> CreateAsync(CreateServiceRequest request)
        {
            var response = await _client.PostAsJsonAsync($"{_controller}/Create", request);
            return await ReadResponse<ServiceInformationResponse>(response);
        }

        public async Task<ServiceInformationResponse?> GetByIdAsync(string id)
        {
            var response = await _client.GetAsync($"{_controller}/GetById?id={id}");
            return await ReadResponse<ServiceInformationResponse>(response);
        }

        public async Task<ServiceInformationResponse?> UpdateAsync(string id, UpdateServiceRequest request)
        {
            var response = await _client.PutAsJsonAsync($"{_controller}/Update/{id}", request);
            return await ReadResponse<ServiceInformationResponse>(response);
        }
    }
}
