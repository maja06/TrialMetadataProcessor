using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System.Text;
using TrialMetadataProcessor.Application.FileValidation.Services;

namespace TrialMetadataProcessor.UnitTests.FileValidation
{
    public class FileValidationServiceTests
    {
        private readonly Mock<ILogger<FileValidationService>> _loggerMock;
        private readonly FileValidationService _service;

        public FileValidationServiceTests()
        {
            _loggerMock = new Mock<ILogger<FileValidationService>>();
            _service = new FileValidationService(_loggerMock.Object);
        }

        [Fact]
        public async Task ValidateFileAndGetContent_WhenFileIsValid_ShouldReturnSuccess()
        {
            var fileMock = new Mock<IFormFile>();
            var jsonContent = @"{
                ""$schema"": ""http://json-schema.org/draft-07/schema#"",
                ""title"": ""ClinicalTrialMetadata"",
                ""type"": ""object"",
                ""properties"": {
                    ""trialId"": ""TEST001"",
                    ""title"": ""Test Trial"",
                    ""startDate"": ""2024-01-01"",
                    ""status"": ""Ongoing"",
                    ""participants"": 100
                },
                ""required"": [
                    ""trialId"",
                    ""title"",
                    ""startDate"",
                    ""status""
                ],
                ""additionalProperties"": false
            }";

            var stream = new MemoryStream(Encoding.UTF8.GetBytes(jsonContent));
            fileMock.Setup(f => f.OpenReadStream()).Returns(stream);
            fileMock.Setup(f => f.Length).Returns(stream.Length);
            fileMock.Setup(f => f.FileName).Returns("valid.json");

            var result = await _service.ValidateFileAndGetContent(fileMock.Object);

            result.ValidationResult.IsValid.Should().BeTrue();
            result.dto.Should().NotBeNull();
        }

        [Fact]
        public async Task ValidateFileAndGetContent_WhenFileSizeExceedsLimit_ShouldReturnError()
        {
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(6 * 1024 * 1024); 
            fileMock.Setup(f => f.FileName).Returns("large.json");

            var result = await _service.ValidateFileAndGetContent(fileMock.Object);

            result.ValidationResult.IsValid.Should().BeFalse();
            result.ValidationResult.ErrorMessage.Should().Contain("size exceeds");
            result.dto.Should().BeNull();
        }
    }
}
