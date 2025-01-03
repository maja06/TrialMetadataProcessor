using System;
using TrialMetadataProcessor.Domain.Interfaces.Repositories.ClinicalTrials;

namespace TrialMetadataProcessor.Domain.Interfaces.UnitOfWork
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IClinicalTrialRepository ClinicalTrials { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}
