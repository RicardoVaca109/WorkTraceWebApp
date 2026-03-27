namespace WorkTrace.WebApp.Models.Dtos.AssignmentEvaluation
{
    public class AssignmentEvaluationDetailResponse
    {
        public string AssignmentId { get; set; }
        public string UserComment { get; set; }
        public string? ClientComment { get; set; }
        public ClientSignatureResponse? ClientSignature { get; set; }
        public List<MediaFileResponse> MediaFiles { get; set; } = new();
        public List<FormEvaluationResponse> Forms { get; set; } = new();
    }

    public class ClientSignatureResponse
    {
        public string Url { get; set; }
        public string SignedBy { get; set; }
        public DateTime SignedAt { get; set; }
    }

    public class MediaFileResponse
    {
        public string Url { get; set; }
        public DateTime UploadedAt { get; set; }
    }

    public class FormEvaluationResponse
    {
        public string FormTemplateId { get; set; }
        public string FormName { get; set; }
        public List<FormEvaluationQuestionResponse> Questions { get; set; } = new();
        public List<UserEvaluationResponse> UserEvaluations { get; set; } = new();
    }

    public class FormEvaluationQuestionResponse
    {
        public string QuestionKey { get; set; }
        public string QuestionText { get; set; }
    }

    public class UserEvaluationResponse
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public int Score { get; set; }
        public List<FormAnswerResponse> Answers { get; set; } = new();
    }

    public class FormAnswerResponse
    {
        public string QuestionKey { get; set; }
        public string QuestionValue { get; set; }
        public string? Answer { get; set; }
        public int? NumericValue { get; set; }
    }
}
