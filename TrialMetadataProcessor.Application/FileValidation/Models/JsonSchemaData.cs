using System.Text.Json;
using System.Text.Json.Serialization;

namespace TrialMetadataProcessor.Application.FileValidation.Models
{
    public class JsonSchemaData
    {
        [JsonPropertyName("$schema")]
        public string Schema { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("properties")]
        public JsonElement Properties { get; set; }
    }
}
