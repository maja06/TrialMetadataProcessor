using MediatR;
using Microsoft.Extensions.Logging;
using TrialMetadataProcessor.Application.ClinicalTrials.DTOs;
using TrialMetadataProcessor.Application.Common.Exceptions;
using TrialMetadataProcessor.Domain.Interfaces.UnitOfWork;

namespace TrialMetadataProcessor.Application.ClinicalTrials.Queries.GetTrialById
{
    public class GetClinicalTrialByIdQueryHandler : IRequestHandler<GetClinicalTrialByIdQuery, GetClinicalTrialDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetClinicalTrialByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetClinicalTrialByIdQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<GetClinicalTrialDto> Handle(GetClinicalTrialByIdQuery request, CancellationToken cancellationToken)
        {
            var trial = await _unitOfWork.ClinicalTrials.GetAsync(request.Id);

            if (trial == null)
                throw new NotFoundException($"Clinical trial with ID {request.Id} not found");

            return new GetClinicalTrialDto
            {
                Id = trial.Id,
                TrialId = trial.TrialId,
                Title = trial.Title,
                StartDate = trial.StartDate,
                EndDate = trial.EndDate,
                Participants = trial.Participants,
                Status = trial.Status,
                DurationInDays = trial.DurationInDays,
                CreationTime = trial.CreationTime,
                ModificationTime = trial.ModificationTime
            };
        }
    }
}
