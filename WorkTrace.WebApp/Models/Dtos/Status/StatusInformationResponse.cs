namespace WorkTrace.WebApp.Models.Dtos.Status
{
    public class StatusInformationResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}