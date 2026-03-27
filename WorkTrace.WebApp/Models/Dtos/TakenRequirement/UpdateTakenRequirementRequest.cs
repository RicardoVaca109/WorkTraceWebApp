namespace WorkTrace.WebApp.Models.Dtos.TakenRequirement
{
    public class UpdateTakenRequirementRequest
    {
        public string Id { get; set; }
        public string ClientId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}