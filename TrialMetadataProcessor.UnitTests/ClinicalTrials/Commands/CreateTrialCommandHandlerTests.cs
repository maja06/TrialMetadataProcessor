using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TrialMetadataProcessor.Application.ClinicalTrials.Commands.CreateClinicalTrial;
using TrialMetadataProcessor.Application.ClinicalTrials.DTOs;
using TrialMetadataProcessor.Domain.Entities.ClinicalTrial;
using TrialMetadataProcessor.Domain.Entities.Enums;
using TrialMetadataProcessor.Domain.Interfaces.Repositories.ClinicalTrials;
using TrialMetadataProcessor.Domain.Interfaces.UnitOfWork;

namespace TrialMetadataProcessor.UnitTests.ClinicalTrials.Commands
{
    public class CreateTrialCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<CreateClinicalTrialCommandHandler>> _loggerMock;
        private readonly CreateClinicalTrialCommandHandler _handler;

        public CreateTrialCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<CreateClinicalTrialCommandHandler>>();
            _handler = new CreateClinicalTrialCommandHandler(_unitOfWorkMock.Object, _loggerMock.Object);

            _unitOfWorkMock.Setup(x => x.ClinicalTrials)
            .Returns(new Mock<IClinicalTrialRepository>().Object);
        }

        [Fact]
        public async Task Handle_WhenValidCommand_ShouldCreateTrial()
        {
            var command = new CreateClinicalTrialCommand
            {
                FileName = "test.json",
                TrialData = new ClinicalTrialDto
                {
                    TrialId = "TEST1",
                    Title = "Test1 Trial",
                    StartDate = DateTime.UtcNow,
                    Status = TrialStatus.Ongoing.ToString(),
                    Participants = 100
                }
            };

            _unitOfWorkMock.Setup(x => x.ClinicalTrials.InsertAsync(It.IsAny<ClinicalTrial>()))
                .ReturnsAsync((ClinicalTrial trial) => trial);

            _unitOfWorkMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            var result = await _handler.Handle(command, CancellationToken.None);

            result.Should().NotBe(Guid.Empty);
            _unitOfWorkMock.Verify(x => x.ClinicalTrials.InsertAsync(It.IsAny<ClinicalTrial>()), Times.Once);
            _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenDuplicateTrialId_ShouldThrowException()
        {
            var command = new CreateClinicalTrialCommand
            {
                FileName = "test.json",
                TrialData = new ClinicalTrialDto
                {
                    TrialId = "TEST2",
                    Title = "Test2 Trial",
                    StartDate = DateTime.UtcNow,
                    Status = TrialStatus.Ongoing.ToString()
                }
            };

            _unitOfWorkMock.Setup(x => x.ClinicalTrials.InsertAsync(It.IsAny<ClinicalTrial>()))
                .ThrowsAsync(new InvalidOperationException("Duplicate TrialId"));

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _handler.Handle(command, CancellationToken.None));
        }
    }
}
