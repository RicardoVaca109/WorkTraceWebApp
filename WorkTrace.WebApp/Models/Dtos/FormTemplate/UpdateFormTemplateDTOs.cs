using WorkTrace.WebApp.Shared;

namespace WorkTrace.WebApp.Models.Dtos.FormTemplate;

public class UpdateFormQuestionsRequest
{
    public string? Id { get; set; }
    public string QuestionKey { get; set; } = string.Empty;
    public string QuestionText { get; set; } = string.Empty;
    public AnswerType AnswerType { get; set; }
}

public class UpdateFormTemplateRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public List<UpdateFormQuestionsRequest> Questions { get; set; } = new();
}
