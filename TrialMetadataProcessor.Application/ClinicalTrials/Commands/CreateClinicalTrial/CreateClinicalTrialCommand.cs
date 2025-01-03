using MediatR;
using TrialMetadataProcessor.Application.ClinicalTrials.DTOs;

namespace TrialMetadataProcessor.Application.ClinicalTrials.Commands.CreateClinicalTrial
{
    public record CreateClinicalTrialCommand : IRequest<Guid>
    {
        public string FileName { get; init; }
        public ClinicalTrialDto TrialData { get; init; }

    }
}
