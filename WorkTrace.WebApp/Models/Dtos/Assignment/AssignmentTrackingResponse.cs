using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace WorkTrace.WebApp.Models.Dtos.Assignment
{
    public class AssignmentTrackingResponse
    {
        public string Id { get; set; }
        public string Client { get; set; }
        public string Service { get; set; }
        public string Address { get; set; }
        public DateTime? CheckIn { get; set; }
        public DateTime? CheckOut { get; set; }
        public DateTime AssignedDate { get; set; }
        public GeoPoint? CurrentLocation { get; set; }
        public GeoPoint? DestinationLocation { get; set; }
        [JsonPropertyName("assignedForms")]
        [JsonProperty("assignedForms")]
        public List<AssignedFormResponse> AssignedForms { get; set; } = new();
    }
}