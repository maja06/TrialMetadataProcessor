using Microsoft.EntityFrameworkCore.Storage;
using TrialMetadataProcessor.Domain.Interfaces.Repositories.ClinicalTrials;
using TrialMetadataProcessor.Domain.Interfaces.UnitOfWork;
using TrialMetadataProcessor.Infrastructure.Data.Context;
using TrialMetadataProcessor.Infrastructure.Repositories.ClinicalTrials;

namespace TrialMetadataProcessor.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction _currentTransaction;

        private IClinicalTrialRepository _clinicalTrials;


        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IClinicalTrialRepository ClinicalTrials =>
               _clinicalTrials ??= new ClinicalTrialRepository(_context);

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task BeginTransactionAsync()
        {

            _currentTransaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await _context.SaveChangesAsync();

                if (_currentTransaction != null)
                {
                    await _currentTransaction.CommitAsync();
                }
            }
            catch
            {
                await RollbackTransactionAsync();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync()
        {
            try
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.RollbackAsync();
                }
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_currentTransaction != null)
                await _currentTransaction.DisposeAsync();
            await _context.DisposeAsync();
        }
    }
}
