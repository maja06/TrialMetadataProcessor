using TrialMetadataProcessor.Domain.Entities.Enums;

namespace TrialMetadataProcessor.Application.ClinicalTrials.DTOs
{
    public class GetClinicalTrialDto
    {
        public Guid Id { get; set; }
        public string TrialId { get; set; }
        public string Title { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public int? Participants { get; set; }
        public TrialStatus Status { get; set; }
        public int DurationInDays { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime ModificationTime { get; set; }
    }
}
