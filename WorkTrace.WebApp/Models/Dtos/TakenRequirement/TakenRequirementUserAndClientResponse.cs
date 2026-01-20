using WorkTrace.WebApp.Models.Dtos.Clients;
using WorkTrace.WebApp.Models.Dtos.Users;

namespace WorkTrace.WebApp.Models.Dtos.TakenRequirement
{
    public class TakenRequirementUserAndClientResponse
    {
        public string Id { get; set; }
        public UserInformationResponse UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public ClientInformationResponse? Client { get; set; }
    }
}