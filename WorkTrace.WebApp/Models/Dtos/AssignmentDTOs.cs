using System.Globalization;
 
namespace WorkTrace.WebApp.Models.Dtos
{
    public class GeoPoint
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
 
    public class AssignmentResponse
    {
        public string Id { get; set; } = string.Empty;
        public List<string> Users { get; set; } = new();
        public string Service { get; set; } = string.Empty;
        public string Client { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime AssignedDate { get; set; }
        public string Address { get; set; } = string.Empty;
        public GeoPoint? DestinationLocation { get; set; }
        public string CreatedByUser { get; set; } = string.Empty;
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
    }
 
    public class CreateAssignmentRequest
    {
        public List<string> Users { get; set; } = new();
        public string Service { get; set; } = string.Empty;
        public string Client { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime AssignedDate { get; set; }
        public string Address { get; set; } = string.Empty;
        public string CreatedByUser { get; set; } = string.Empty;
    }
 
    public class UpdateAssignmentRequest
    {
        public List<string>? Users { get; set; }
        public string? Service { get; set; }
        public string? Client { get; set; }
        public string? Status { get; set; }
        public DateTime? AssignedDate { get; set; }
        public string? Address { get; set; }
    }
 
    public class ClientHistoryResponse
    {
        public string Service { get; set; } = string.Empty;
        public DateTime AssignedDate { get; set; }
        public DateTime? CheckOut { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public List<string> Users { get; set; } = new();
    }
}