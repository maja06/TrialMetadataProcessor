using TrialMetadataProcessor.Domain.Entities.Base;
using TrialMetadataProcessor.Domain.Entities.Enums;

namespace TrialMetadataProcessor.Domain.Entities.ClinicalTrial
{
    public class ClinicalTrial : Entity<Guid>
    {
        public string TrialId { get; private set; }
        public string Title { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }
        public int? Participants { get; private set; }
        public TrialStatus Status { get; private set; }
        public int DurationInDays { get; private set; }
        public DateTime CreationTime { get; private set; }
        public DateTime ModificationTime { get; private set; }

        private ClinicalTrial() { }

        public ClinicalTrial(
            string trialId,
            string title,
            DateTime startDate,
            TrialStatus status,
            int? participants = null,
            DateTime? endDate = null)
        {
            ValidateConstructorParameters(trialId, title, startDate, status, participants, endDate);
            ValidateBusinessRules(status, startDate, endDate);

            Id = Guid.NewGuid();
            TrialId = trialId;
            Title = title;
            StartDate = startDate;
            Status = status;
            Participants = participants;
            EndDate = DetermineEndDate(endDate, startDate, status);
            CreationTime = DateTime.UtcNow;
            ModificationTime = DateTime.UtcNow;
            DurationInDays = CalculateDuration();
        }

        private void ValidateConstructorParameters(
        string trialId,
        string title,
        DateTime startDate,
        TrialStatus status,
        int? participants,
        DateTime? endDate)
        {
            if (string.IsNullOrEmpty(trialId))
                throw new ArgumentException("TrialId cannot be empty");

            if (string.IsNullOrEmpty(title))
                throw new ArgumentException("Title cannot be empty");

            if (startDate == default)
                throw new ArgumentException("StartDate must be provided");

            if (participants.HasValue && participants.Value < 0)
                throw new ArgumentException("Participants cannot be negative");

            if (endDate.HasValue && endDate.Value < startDate)
                throw new ArgumentException("EndDate cannot be before StartDate");

            if (!Enum.IsDefined(typeof(TrialStatus), status))
                throw new ArgumentException("Invalid trial status");
        }

        private void ValidateBusinessRules(TrialStatus status, DateTime startDate, DateTime? endDate)
        {
            if (status == TrialStatus.Completed && !endDate.HasValue)
                throw new InvalidOperationException("Completed trials must have an end date");

            if (status == TrialStatus.NotStarted && startDate.Date < DateTime.UtcNow.Date)
                throw new InvalidOperationException("Cannot set NotStarted status for trials with past start date");

            if (endDate.HasValue && endDate.Value.Date > DateTime.UtcNow.Date)
                throw new ArgumentException("EndDate cannot be in the future");
        }

        private DateTime? DetermineEndDate(DateTime? endDate, DateTime startDate, TrialStatus status)
        {
            if (endDate.HasValue)
                return endDate;

            return status == TrialStatus.Ongoing
                ? startDate.AddMonths(1)
                : null;
        }

        private int CalculateDuration()
        {
            var end = EndDate ?? DateTime.UtcNow;
            return (end - StartDate).Days;
        }

        public void UpdateEndDate(DateTime endDate)
        {
            if (endDate < StartDate)
                throw new ArgumentException("EndDate cannot be before StartDate");

            EndDate = endDate;
            DurationInDays = CalculateDuration();
            ModificationTime = DateTime.UtcNow;
        }
    }
}
