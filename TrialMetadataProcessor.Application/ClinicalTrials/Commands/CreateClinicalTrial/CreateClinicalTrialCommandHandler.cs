using MediatR;
using Microsoft.Extensions.Logging;
using TrialMetadataProcessor.Domain.Entities.ClinicalTrial;
using TrialMetadataProcessor.Domain.Entities.Enums;
using TrialMetadataProcessor.Domain.Interfaces.UnitOfWork;

namespace TrialMetadataProcessor.Application.ClinicalTrials.Commands.CreateClinicalTrial
{
    public class CreateClinicalTrialCommandHandler : IRequestHandler<CreateClinicalTrialCommand, Guid>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CreateClinicalTrialCommandHandler> _logger;

        public CreateClinicalTrialCommandHandler(IUnitOfWork unitOfWork,
             ILogger<CreateClinicalTrialCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Guid> Handle(CreateClinicalTrialCommand request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Processing file: {FileName}", request.FileName);

                var trial = new ClinicalTrial(
                        request.TrialData.TrialId,
                        request.TrialData.Title,
                        request.TrialData.StartDate,
                        (TrialStatus)Enum.Parse(typeof(TrialStatus), request.TrialData?.Status),
                        request.TrialData.Participants,
                        request.TrialData.EndDate
                    );

                await _unitOfWork.ClinicalTrials.InsertAsync(trial);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Successfully created trial with ID: {Id}", trial.Id);

                return trial.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating trial from file: {FileName}", request.FileName);
                throw;
            }
        }
    }
}
