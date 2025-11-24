using System.Collections.Generic;

namespace WorkTrace.WebApp.Models.Dtos.Service
{
    public class UpdateServiceRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<UpdateInstallationStepRequest> InstallationSteps { get; set; }
    }

    public class UpdateInstallationStepRequest
    {
        public string Id { get; set; }
        public int Steps { get; set; }
        public string Description { get; set; }
    }
}
