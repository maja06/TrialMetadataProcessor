using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TrialMetadataProcessor.Domain.Entities.ClinicalTrial;

namespace TrialMetadataProcessor.Infrastructure.Data.Configurations
{
    public class ClinicalTrialConfiguration : IEntityTypeConfiguration<ClinicalTrial>
    {
        public void Configure(EntityTypeBuilder<ClinicalTrial> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.TrialId)
                .HasMaxLength(50)
                .IsRequired();

            builder.HasIndex(t => t.TrialId)
                .IsUnique();

            builder.Property(t => t.Title)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(t => t.StartDate)
                .IsRequired();

            builder.Property(t => t.EndDate)
                .IsRequired(false);

            builder.Property(t => t.Participants)
                .IsRequired(false)
                .HasDefaultValue(null);

            builder.Property(t => t.Status)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(t => t.DurationInDays)
                .IsRequired();

            builder.Property(t => t.CreationTime)
                .IsRequired();

            builder.Property(t => t.ModificationTime)
                .IsRequired();
        }
    }
}
