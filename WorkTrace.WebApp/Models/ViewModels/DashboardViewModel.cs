using WorkTrace.WebApp.Models.Dtos.FormTemplate;
using WorkTrace.WebApp.Models.Dtos.Users;

namespace WorkTrace.WebApp.Models.ViewModels;

public class DashboardViewModel
{
    public List<UserInformationResponse> Users { get; set; } = new();
    public List<FormTemplateResponse> Templates { get; set; } = new();
}
