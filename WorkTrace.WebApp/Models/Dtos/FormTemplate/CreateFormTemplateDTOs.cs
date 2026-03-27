using WorkTrace.WebApp.Shared;

namespace WorkTrace.WebApp.Models.Dtos.FormTemplate;

public class CreateFormQuestionRequest
{
    public string QuestionKey { get; set; } = string.Empty;
    public string QuestionText { get; set; } = string.Empty;
    public AnswerType AnswerType { get; set; }
}

public class CreateFormTemplateRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<CreateFormQuestionRequest> Questions { get; set; } = new();
    public bool IsActive { get; set; } = true;
}
