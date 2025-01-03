using MediatR;
using TrialMetadataProcessor.Application.ClinicalTrials.DTOs;
using TrialMetadataProcessor.Application.Common.Queries;

namespace TrialMetadataProcessor.Application.ClinicalTrials.Queries.GetTrialsByFilterQuery
{
    public class GetTrialsByFilterQuery : IRequest<IEnumerable<GetClinicalTrialDto>>
    {
        public FilterInfo Filter { get; set; }
        public SearchInfo Search { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
    }
}
