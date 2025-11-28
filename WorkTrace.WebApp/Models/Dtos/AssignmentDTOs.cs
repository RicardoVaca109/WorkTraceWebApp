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
        public string AssignedDate { get; set; } = string.Empty;
        public string AssignedTime { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public GeoPoint? DestinationLocation { get; set; }
        public string CreatedByUser { get; set; } = string.Empty;
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }

        // Computed property for Date, combining AssignedDate and AssignedTime
        public DateTime Date
        {
            get
            {
                if (DateTime.TryParseExact($"{AssignedDate} {AssignedTime}", "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var parsedDateTime))
                {
                    return parsedDateTime;
                }
                return default;
            }
        }
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
        public string? AssignedTime { get; set; }
        public string? Address { get; set; }
        public string? CreatedByUser { get; set; }
    }

    public class ClientHistoryResponse
    {
        public string Service { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string Time { get; set; } = string.Empty;
        public string CheckOutDate { get; set; } = string.Empty;
        public string CheckOutTime { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public List<string> Users { get; set; } = new();

        public DateTime GetDate()
        {
            if (DateTime.TryParseExact($"{Date} {Time}", "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDateTime))
            {
                return parsedDateTime;
            }
            // Fallback for just date if time is missing
            if (DateTime.TryParseExact(Date, "dd-MM-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out parsedDateTime))
            {
                return parsedDateTime;
            }
            return default;
        }

        public DateTime? GetCheckOutDate()
        {
            if (string.IsNullOrWhiteSpace(CheckOutDate) || string.IsNullOrWhiteSpace(CheckOutTime))
            {
                return null;
            }
            if (DateTime.TryParseExact($"{CheckOutDate} {CheckOutTime}", "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDateTime))
            {
                return parsedDateTime;
            }
            return null;
        }
    }
}