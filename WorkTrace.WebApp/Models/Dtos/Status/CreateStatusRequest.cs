namespace WorkTrace.WebApp.Models.Dtos.Status;

public class CreateStatusRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}