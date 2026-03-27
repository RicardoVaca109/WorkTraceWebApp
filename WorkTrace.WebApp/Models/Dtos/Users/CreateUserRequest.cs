using WorkTrace.WebApp.Shared;

namespace WorkTrace.WebApp.Models.Dtos.Users
{
    public class CreateUserRequest
    {
        public string FullName { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserRoles Role { get; set; }
        public bool IsActive { get; set; } = true;
    }
}