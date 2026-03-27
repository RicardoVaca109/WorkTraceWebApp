using WorkTrace.WebApp.Models.Dtos.AssignmentEvaluation;
using WorkTrace.WebApp.Services.Interfaces;

namespace WorkTrace.WebApp.Services.ApiServices
{
    public class AssignmentEvaluationApiService : BaseApiService, IAssignmentEvaluationApiService
    {
        private readonly string _controller = "AssignmentEvaluation";

        public AssignmentEvaluationApiService(HttpClient client) : base(client)
        {
        }

        public async Task<AssignmentEvaluationDetailResponse?> GetEvaluationDetailByAssignmentAsync(string assignmentId)
        {
            var response = await _client.GetAsync($"{_controller}/GetDetailForAssignment/detail/{assignmentId}");
            return await ReadResponse<AssignmentEvaluationDetailResponse>(response);
        }
    }
}
