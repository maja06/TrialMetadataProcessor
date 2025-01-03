using System;
using TrialMetadataProcessor.Domain.Entities.ClinicalTrial;
using TrialMetadataProcessor.Domain.Entities.Enums;
using TrialMetadataProcessor.Domain.Interfaces.Repositories.Base;

namespace TrialMetadataProcessor.Domain.Interfaces.Repositories.ClinicalTrials
{
    public interface IClinicalTrialRepository : IRepository<ClinicalTrial, Guid>
    {
        Task<bool> ExistsAsync(string trialId);
        Task<ClinicalTrial?> GetByTrialIdAsync(string trialId);
        Task<IEnumerable<ClinicalTrial>> GetByStatusAsync(TrialStatus status);
    }
}
