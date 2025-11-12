namespace WorkTrace.WebApp.Models.Dtos.Users;

public class LoginResponse
{
    public string Token { get; set; }
    public DateTime ExpireAt { get; set; }
}
