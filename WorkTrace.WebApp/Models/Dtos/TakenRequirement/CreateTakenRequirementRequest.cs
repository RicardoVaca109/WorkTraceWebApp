namespace WorkTrace.WebApp.Models.Dtos.TakenRequirement
{
    public class CreateTakenRequirementRequest
    {
        public string UserId { get; set; }
        public string ClientId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}