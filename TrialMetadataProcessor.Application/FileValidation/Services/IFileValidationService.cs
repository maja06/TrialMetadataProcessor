using Microsoft.AspNetCore.Http;
using TrialMetadataProcessor.Application.ClinicalTrials.DTOs;
using TrialMetadataProcessor.Application.Common.Models;

namespace TrialMetadataProcessor.Application.FileValidation.Services
{
    public interface IFileValidationService
    {
        Task<(FileValidationResult ValidationResult, ClinicalTrialDto dto)> ValidateFileAndGetContent(IFormFile file);
    }
}