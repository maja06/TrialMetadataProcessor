using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using TrialMetadataProcessor.Application.Common.Models;
using TrialMetadataProcessor.Application.FileValidation.Constants;
using TrialMetadataProcessor.Application.FileValidation.Models;
using System.ComponentModel.DataAnnotations;
using TrialMetadataProcessor.Application.ClinicalTrials.DTOs;
using TrialMetadataProcessor.Application.Common.Converters;
using TrialMetadataProcessor.Application.Common.Extensions;

namespace TrialMetadataProcessor.Application.FileValidation.Services
{
    public class FileValidationService : IFileValidationService
    {
        private readonly ILogger<FileValidationService> _logger;

        public FileValidationService(ILogger<FileValidationService> logger)
        {
            _logger = logger;
        }

        public async Task<(FileValidationResult ValidationResult, ClinicalTrialDto dto)> ValidateFileAndGetContent(IFormFile file)
        {
            var fileValidation = ValidateFileProperties(file);
            if (!fileValidation.IsValid)
            {
                return (fileValidation, null);
            }

            return await ValidateJsonContentAndStructure(file);
        }

        private FileValidationResult ValidateFileProperties(IFormFile file)
        {
            _logger.LogInformation("Validating file properties for {FileName}", file.FileName);

            if (file == null || file.Length <= 0)
            {
                _logger.LogWarning("File size validation failed. File is empty.");
                return FileValidationResult.Error(FileValidationMessages.FileEmpty);
            }


            if (file.Length > FileValidationConstants.MaxFileSize)
            {
                _logger.LogWarning("File size validation failed. Size: {Size}", file.Length);
                return FileValidationResult.Error(FileValidationMessages.FileSizeExceeded);
            }

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (extension != FileValidationConstants.AllowedExtension)
            {
                _logger.LogWarning("File type validation failed. Extension: {Extension}", extension);
                return FileValidationResult.Error(FileValidationMessages.InvalidFileType);
            }

            return FileValidationResult.Success();
        }

        private async Task<(FileValidationResult, ClinicalTrialDto dto)> ValidateJsonContentAndStructure(IFormFile file)
        {
            try
            {
                _logger.LogInformation("Validating JSON content for {FileName}", file.FileName);

                 var fileContent = await file.ReadAsStringAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    Converters =
                    {
                        new JsonStringDateTimeConverter()
                    }};

                var schemaData = JsonSerializer.Deserialize<JsonSchemaData>(fileContent, options);
                if (schemaData == null)
                {
                    return (FileValidationResult.Error("Invalid schema format"), null);
                }

                var propertiesJson = schemaData.Properties.GetRawText();

                try
                {
                    var trialDto = JsonSerializer.Deserialize<ClinicalTrialDto>(propertiesJson, options);

                    if (trialDto == null)
                    {
                        return (FileValidationResult.Error("Invalid trial data format"), null);
                    }

                    var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(trialDto);
                    var validationResults = new List<ValidationResult>();

                    if (!Validator.TryValidateObject(trialDto, validationContext, validationResults, validateAllProperties: true))
                    {
                        var errors = string.Join(", ", validationResults.Select(r => r.ErrorMessage));
                        return (FileValidationResult.Error(errors), null);
                    }

                    return (FileValidationResult.Success(), trialDto);
                }
                catch (JsonException ex)
                {
                    _logger.LogError(ex, "Error deserializing properties for {FileName}", file.FileName);
                    return (FileValidationResult.Error("Invalid properties format"), null);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error validating JSON for {FileName}", file.FileName);
                return (FileValidationResult.Error("Error validating file content"), null);
            }
        }
    }
}

