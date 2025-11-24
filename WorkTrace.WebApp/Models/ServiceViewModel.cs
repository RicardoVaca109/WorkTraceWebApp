using WorkTrace.WebApp.Models.Dtos.Service;

namespace WorkTrace.WebApp.Models
{
    public class ServiceViewModel
    {
        public List<ServiceInformationResponse> Services { get; set; }
        public CreateServiceRequest NewService { get; set; }
    }
}
