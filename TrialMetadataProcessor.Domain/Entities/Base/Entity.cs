using System;
using System.ComponentModel.DataAnnotations;

namespace TrialMetadataProcessor.Domain.Entities.Base
{
    public abstract class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        [Key]
        public TPrimaryKey Id { get; set; }

    }
}
