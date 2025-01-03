using Microsoft.EntityFrameworkCore;
using TrialMetadataProcessor.Domain.Entities.ClinicalTrial;
using TrialMetadataProcessor.Domain.Entities.Enums;
using TrialMetadataProcessor.Domain.Interfaces.Repositories.ClinicalTrials;
using TrialMetadataProcessor.Infrastructure.Data.Context;
using TrialMetadataProcessor.Infrastructure.Repositories.Base;

namespace TrialMetadataProcessor.Infrastructure.Repositories.ClinicalTrials
{
    public class ClinicalTrialRepository : Repository<ClinicalTrial, Guid>, IClinicalTrialRepository
    {
        public ClinicalTrialRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<bool> ExistsAsync(string trialId)
        {
            return await _dbSet.AnyAsync(t => t.TrialId == trialId);
        }

        public async Task<ClinicalTrial?> GetByTrialIdAsync(string trialId)
        {
            return await _context.ClinicalTrials
                .FirstOrDefaultAsync(x => x.TrialId == trialId);
        }

        public async Task<IEnumerable<ClinicalTrial>> GetByStatusAsync(TrialStatus status)
        {
            return await _dbSet
                .Where(t => t.Status == status)
                .ToListAsync();
        }
    }

}
