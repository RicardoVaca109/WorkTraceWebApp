namespace WorkTrace.WebApp.Models.Dtos.Service
{
    public class ServiceInformationResponse
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public List<InstallationStepResponse> InstallationSteps { get; set; } = new();
    }
}
