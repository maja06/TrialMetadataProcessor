using System;
using System.Text.Json.Serialization;
using TrialMetadataProcessor.Application.Common.Converters;

namespace TrialMetadataProcessor.Application.Common.Queries
{
    public class RuleInfo
    {
        public string Property { get; init; }
        public string Operator { get; init; } 
        public string Value { get; init; }

        [JsonConverter(typeof(StringEnumConverter<FilterCondition>))]
        public FilterCondition? Condition { get; init; }
        public List<RuleInfo> Rules { get; set; } = new List<RuleInfo>();
    }
}
