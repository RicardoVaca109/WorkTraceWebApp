namespace WorkTrace.WebApp.Models.Dtos.Service
{
    public class CreateServiceRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<InstallationStepResponse> InstallationSteps { get; set; }
    }
}
