using WorkTrace.WebApp.Shared;

namespace WorkTrace.WebApp.Models.Dtos.Users
{
    public class UserInformationResponse
    {
        public string Id { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserRoles Role { get; set; }
        public bool IsActive { get; set; }
    }
}