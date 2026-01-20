namespace WorkTrace.WebApp.Models.Dtos.KPIs;

public class EvaluationDashboardResponse
{
    public string UserId { get; set; }
    public string UserName { get; set; }
    public int TotalEvaluations { get; set; }
    public List<DashboardQuestionMetricResponse> Metrics { get; set; }
}

public class DashboardQuestionMetricResponse
{
    public string QuestionKey { get; set; }
    public string QuestionText { get; set; }
    public double Average { get; set; }
    public int Count { get; set; }
}
