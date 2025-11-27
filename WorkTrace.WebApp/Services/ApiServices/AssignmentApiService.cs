using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using WorkTrace.WebApp.Models.Dtos;
using WorkTrace.WebApp.Models.Dtos.Assignment;
using WorkTrace.WebApp.Services.Interfaces;

namespace WorkTrace.WebApp.Services.ApiServices
{
    public class AssignmentApiService : BaseApiService, IAssignmentApiService
    {
        private readonly string _controller = "Assignment";

        public AssignmentApiService(HttpClient client) : base(client)
        {
        }

        public async Task<List<AssignmentResponse>?> GetAllAsync()
        {
            var response = await _client.GetAsync($"{_controller}/GetAll?_={DateTime.UtcNow.Ticks}");
            return await ReadResponse<List<AssignmentResponse>>(response);
        }

        public async Task<List<ClientHistoryResponse>?> GetClientHistoryAsync(string clientId)
        {
            var response = await _client.GetAsync($"{_controller}/GetClientHistory/{clientId}");
            return await ReadResponse<List<ClientHistoryResponse>>(response);
        }

        public async Task<AssignmentResponse?> CreateAsync(CreateAssignmentRequest request)
        {
            var response = await _client.PostAsJsonAsync($"{_controller}/Create", request);
            return await ReadResponse<AssignmentResponse>(response);
        }

        public async Task<AssignmentResponse?> UpdateAsync(string id, UpdateAssignmentRequest request)
        {
            var response = await _client.PutAsJsonAsync($"{_controller}/UpdateAssignment/{id}", request);
            return await ReadResponse<AssignmentResponse>(response);
        }

        public async Task<AssignmentResponse?> GetByIdAsync(string id)
        {
            var response = await _client.GetAsync($"{_controller}/GetById?id={id}");
            return await ReadResponse<AssignmentResponse>(response);
        }

        public async Task<List<AssignmentListResponse>?> GetAssignmentsListAsync(string userId)
        {
            var response = await _client.GetAsync($"{_controller}/GetAssignmentsList/by-user/{userId}");
            return await ReadResponse<List<AssignmentListResponse>>(response);
        }

        public async Task<AssignmentTrackingResponse?> GetAssignmentTrackingAsync(string assignmentId)
        {
            var response = await _client.GetAsync($"{_controller}/GetAssignmentTracking/tracking/{assignmentId}");
            return await ReadResponse<AssignmentTrackingResponse>(response);
        }
    }
}
