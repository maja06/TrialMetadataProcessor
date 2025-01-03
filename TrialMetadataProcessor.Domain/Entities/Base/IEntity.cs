
namespace TrialMetadataProcessor.Domain.Entities.Base
{
    public interface IEntity<TPrimaryKey>
    {
        TPrimaryKey Id { get; set; }
    }
}
