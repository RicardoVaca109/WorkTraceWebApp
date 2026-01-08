using WorkTrace.WebApp.Shared;

namespace WorkTrace.WebApp.Models.Dtos.FormTemplate;

public class FormQuestionResponse
{
    public string Id { get; set; } = string.Empty;
    public string QuestionKey { get; set; } = string.Empty;
    public string QuestionText { get; set; } = string.Empty;
    public AnswerType AnswerType { get; set; }
}

public class FormTemplateResponse
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<FormQuestionResponse> Questions { get; set; } = new();
}
