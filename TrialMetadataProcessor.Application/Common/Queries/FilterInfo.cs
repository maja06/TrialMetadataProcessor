using System.Text.Json.Serialization;
using TrialMetadataProcessor.Application.Common.Converters;

namespace TrialMetadataProcessor.Application.Common.Queries
{
    public class FilterInfo
    {
        [JsonConverter(typeof(StringEnumConverter<FilterCondition>))]
        public FilterCondition Condition { get; init; }
        public ICollection<RuleInfo> Rules { get; init; } = new List<RuleInfo>();
    }
}
