namespace WorkTrace.WebApp.Models.Dtos.Status;

public class CreateStatusRequest
{
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
}