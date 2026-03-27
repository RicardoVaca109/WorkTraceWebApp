using WorkTrace.WebApp.Models.Dtos.Status;
using WorkTrace.WebApp.Services.Interfaces;

namespace WorkTrace.WebApp.Services.ApiServices
{
    public class StatusApiService : BaseApiService, IStatusApiService
    {
        private readonly string _controller = "Status";

        public StatusApiService(HttpClient client) : base(client)
        {
        }

        public async Task<List<StatusInformationResponse>?> GetAllAsync()
        {
            var response = await _client.GetAsync($"{_controller}/GetAll");
            return await ReadResponse<List<StatusInformationResponse>>(response);
        }

        public async Task<StatusInformationResponse?> CreateAsync(CreateStatusRequest request)
        {
            var response = await _client.PostAsJsonAsync($"{_controller}/Create", request);
            return await ReadResponse<StatusInformationResponse>(response);
        }
    }
}
