using Microsoft.EntityFrameworkCore;
using TrialMetadataProcessor.Domain.Entities.ClinicalTrial;
using TrialMetadataProcessor.Infrastructure.Data.Configurations;

namespace TrialMetadataProcessor.Infrastructure.Data.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
       : base(options)
        {
        }

        public DbSet<ClinicalTrial> ClinicalTrials => Set<ClinicalTrial>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClinicalTrialConfiguration());
        }
    }
}
