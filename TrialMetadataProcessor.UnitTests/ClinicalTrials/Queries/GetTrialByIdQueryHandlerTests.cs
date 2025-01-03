using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TrialMetadataProcessor.Application.ClinicalTrials.Queries.GetTrialById;
using TrialMetadataProcessor.Domain.Entities.ClinicalTrial;
using TrialMetadataProcessor.Domain.Entities.Enums;
using TrialMetadataProcessor.Domain.Interfaces.UnitOfWork;

namespace TrialMetadataProcessor.UnitTests.ClinicalTrials.Queries
{
    public class GetTrialByIdQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<GetClinicalTrialByIdQueryHandler>> _loggerMock;
        private readonly GetClinicalTrialByIdQueryHandler _handler;

        public GetTrialByIdQueryHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<GetClinicalTrialByIdQueryHandler>>();
            _handler = new GetClinicalTrialByIdQueryHandler(_unitOfWorkMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_WithValidId_ReturnsTrialDto()
        {
            var id = Guid.NewGuid();
            var trial = new ClinicalTrial(
                "T1",
                "Test Trial",
                DateTime.Now,
                TrialStatus.Ongoing);

            _unitOfWorkMock.Setup(x => x.ClinicalTrials.GetAsync(id))
                .ReturnsAsync(trial);

            var result = await _handler.Handle(new GetClinicalTrialByIdQuery(id), CancellationToken.None);

            result.Should().NotBeNull();
            result.TrialId.Should().Be("T1");
            result.Title.Should().Be("Test Trial");
        }
    }
}
