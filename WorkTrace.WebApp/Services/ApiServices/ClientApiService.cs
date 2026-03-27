using WorkTrace.WebApp.Models.Dtos.Clients;
using WorkTrace.WebApp.Services.Interfaces;

namespace WorkTrace.WebApp.Services.ApiServices
{
    public class ClientApiService : BaseApiService, IClientApiService
    {
        private readonly string _controller = "Client";

        public ClientApiService(HttpClient client) : base(client)
        {
        }

        public async Task<List<ClientInformationResponse>?> GetAllAsync()
        {
            var response = await _client.GetAsync($"{_controller}/GetAll");
            return await ReadResponse<List<ClientInformationResponse>>(response);
        }

        public async Task<ClientInformationResponse?> CreateAsync(CreateClientRequest request)
        {
            var response = await _client.PostAsJsonAsync($"{_controller}/Create", request);
            return await ReadResponse<ClientInformationResponse>(response);
        }

        public async Task<ClientInformationResponse?> UpdateAsync(string id, UpdateClientRequest request)
        {
            var response = await _client.PutAsJsonAsync($"{_controller}/Update/{id}", request);
            return await ReadResponse<ClientInformationResponse>(response);
        }

        public async Task<ClientInformationResponse?> GetByIdAsync(string id)
        {
            var response = await _client.GetAsync($"{_controller}/GetById?id={id}");
            return await ReadResponse<ClientInformationResponse>(response);
        }
    }
}
