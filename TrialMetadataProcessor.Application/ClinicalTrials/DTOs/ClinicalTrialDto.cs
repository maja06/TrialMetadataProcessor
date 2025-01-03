using System.ComponentModel.DataAnnotations;

namespace TrialMetadataProcessor.Application.ClinicalTrials.DTOs
{
    public class ClinicalTrialDto
    {
        [Required(ErrorMessage = "TrialId is required")]
        public string TrialId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage = "StartDate is required")]
        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Participants must be greater than 0")]
        public int? Participants { get; set; }

        [Required(ErrorMessage = "Status is required")]
        [RegularExpression("^(Not Started|Ongoing|Completed)$",
            ErrorMessage = "Status must be one of: Not Started, Ongoing, Completed")]
        public string Status { get; set; }
    }
}
