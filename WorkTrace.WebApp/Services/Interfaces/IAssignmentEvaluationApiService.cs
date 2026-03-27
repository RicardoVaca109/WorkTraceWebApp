using WorkTrace.WebApp.Models.Dtos.AssignmentEvaluation;

namespace WorkTrace.WebApp.Services.Interfaces
{
    public interface IAssignmentEvaluationApiService
    {
        Task<AssignmentEvaluationDetailResponse?> GetEvaluationDetailByAssignmentAsync(string assignmentId);
    }
}
