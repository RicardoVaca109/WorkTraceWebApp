using WorkTrace.WebApp.Models.Dtos.Clients;
using WorkTrace.WebApp.Models.Dtos.Service;
using WorkTrace.WebApp.Models.Dtos.Status;
using WorkTrace.WebApp.Models.Dtos.Users;
using WorkTrace.WebApp.Models.Dtos.FormTemplate;

namespace WorkTrace.WebApp.Models
{
    public class CalendarioActividadesViewModel
    {
        public string? EventsJson { get; set; }
        public List<UserInformationResponse> Users { get; set; } = new();
        public List<ClientInformationResponse> Clients { get; set; } = new();
        public List<ServiceInformationResponse> Services { get; set; } = new();
        public List<StatusInformationResponse> Statuses { get; set; } = new();
        public List<FormTemplateResponse> FormTemplates { get; set; } = new();
        public string? LoggedInUserId { get; set; }
        public string? UsersJson { get; set; }
        public string? ClientsJson { get; set; }
        public string? ServicesJson { get; set; }
        public string? StatusesJson { get; set; }
        public string? FormTemplatesJson { get; set; }
    }
}
