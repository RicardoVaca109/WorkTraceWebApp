using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace WorkTrace.WebApp.Models.Dtos
{
    public class AssignedFormResponse
    {
        [JsonPropertyName("id")]
        [JsonProperty("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("name")]
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;
    }
}