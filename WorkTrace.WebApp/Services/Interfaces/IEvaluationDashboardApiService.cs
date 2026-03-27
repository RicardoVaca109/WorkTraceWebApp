using WorkTrace.WebApp.Models.Dtos.KPIs;

namespace WorkTrace.WebApp.Services.Interfaces;

public interface IEvaluationDashboardApiService
{
    Task<EvaluationDashboardResponse?> GetDashboardAsync(string userId, DateTime start, DateTime end);
}
