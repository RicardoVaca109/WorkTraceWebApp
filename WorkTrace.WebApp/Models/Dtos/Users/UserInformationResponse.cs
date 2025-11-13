using WorkTrace.WebApp.Shared;

namespace WorkTrace.WebApp.Models.Dtos.Users
{
    public class UserInformationResponse
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string DocumentNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public UserRoles Role { get; set; }
        public bool IsActive { get; set; }
    }
}