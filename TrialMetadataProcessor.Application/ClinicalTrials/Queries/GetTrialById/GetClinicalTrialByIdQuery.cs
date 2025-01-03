using MediatR;
using TrialMetadataProcessor.Application.ClinicalTrials.DTOs;

namespace TrialMetadataProcessor.Application.ClinicalTrials.Queries.GetTrialById
{
    public record GetClinicalTrialByIdQuery(Guid Id) : IRequest<GetClinicalTrialDto>;
}
