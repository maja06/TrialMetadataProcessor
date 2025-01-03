
namespace TrialMetadataProcessor.Application.FileValidation.Models
{
    public class JsonSchemaProperties
    {
        public string TrialId { get; set; }
        public string Title { get; set; }
        public string StartDate { get; set; }
        public string Status { get; set; }
        public int Participants { get; set; }
        public string EndDate { get; set; }
    }
}
