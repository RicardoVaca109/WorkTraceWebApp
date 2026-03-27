namespace WorkTrace.WebApp.Models.Dtos.Service
{
    public class InstallationStepResponse
    {
        public string Id { get; set; } = string.Empty;
        public int Steps { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}
