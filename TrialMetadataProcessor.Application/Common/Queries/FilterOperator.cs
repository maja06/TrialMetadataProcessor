using System.Text.Json.Serialization;

namespace TrialMetadataProcessor.Application.Common.Queries
{
    public enum FilterOperator
    {
        [JsonPropertyName("equals")]
        Equals,
        [JsonPropertyName("greaterThan")]
        GreaterThan,
        [JsonPropertyName("lessThan")]
        LessThan,
        [JsonPropertyName("contains")]
        Contains,
        [JsonPropertyName("startsWith")]
        StartsWith,
        [JsonPropertyName("endsWith")]
        EndsWith
    }
}
