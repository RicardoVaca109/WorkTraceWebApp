namespace WorkTrace.WebApp.Models.Dtos.Clients;

public class UpdateClientRequest
{
    public string? FullName { get; set; }
    public string? DocumentNumber { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
}