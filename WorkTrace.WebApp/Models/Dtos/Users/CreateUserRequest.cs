namespace WorkTrace.WebApp.Models.Dtos.Users;

public class CreateUserRequest
{
    public string FullName { get; set; }
    public string DocumentNumber { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public bool IsActive { get; set; }
}