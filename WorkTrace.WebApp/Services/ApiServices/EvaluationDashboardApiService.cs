using WorkTrace.WebApp.Models.Dtos.KPIs;
using WorkTrace.WebApp.Services.Interfaces;

namespace WorkTrace.WebApp.Services.ApiServices;

public class EvaluationDashboardApiService : BaseApiService, IEvaluationDashboardApiService
{
    private readonly string _controller = "EvaluationDashboard";

    public EvaluationDashboardApiService(HttpClient client) : base(client)
    {
    }

    public async Task<EvaluationDashboardResponse?> GetDashboardAsync(string userId, DateTime start, DateTime end)
    {
        // Format dates as ISO string to avoid culture issues
        var startStr = start.ToString("yyyy-MM-ddTHH:mm:ss");
        var endStr = end.ToString("yyyy-MM-ddTHH:mm:ss");
        
        var response = await _client.GetAsync($"{_controller}/GetDashboard?userId={userId}&start={startStr}&end={endStr}");
        return await ReadResponse<EvaluationDashboardResponse>(response);
    }
}
