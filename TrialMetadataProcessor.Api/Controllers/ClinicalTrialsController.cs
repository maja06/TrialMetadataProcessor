using MediatR;
using Microsoft.AspNetCore.Mvc;
using TrialMetadataProcessor.Application.ClinicalTrials.Commands.CreateClinicalTrial;
using TrialMetadataProcessor.Application.ClinicalTrials.DTOs;
using TrialMetadataProcessor.Application.ClinicalTrials.Queries.GetTrialById;
using TrialMetadataProcessor.Application.ClinicalTrials.Queries.GetTrialsByFilterQuery;
using TrialMetadataProcessor.Application.ClinicalTrials.Queries.GetTrialsByStatus;
using TrialMetadataProcessor.Application.Common.Exceptions;
using TrialMetadataProcessor.Application.FileValidation.Services;
using TrialMetadataProcessor.Domain.Entities.Enums;

namespace TrialMetadataProcessor.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClinicalTrialsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IFileValidationService _fileValidationService;
        private readonly ILogger<ClinicalTrialsController> _logger;

        public ClinicalTrialsController(
            IMediator mediator,
            IFileValidationService fileValidationService,
            ILogger<ClinicalTrialsController> logger)
        {
            _mediator = mediator;
            _fileValidationService = fileValidationService;
            _logger = logger;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            try
            {
                var fileValidationResult = await _fileValidationService.ValidateFileAndGetContent(file);
                if (!fileValidationResult.ValidationResult.IsValid)
                {
                    return BadRequest(fileValidationResult.ValidationResult.ErrorMessage);
                }

                var command = new CreateClinicalTrialCommand
                {
                    TrialData = fileValidationResult.dto,
                    FileName = file.FileName
                };

                var result = await _mediator.Send(command);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing file upload");
                return StatusCode(500, "An error occurred while processing the file.");
            }
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ClinicalTrialDto>> GetById(Guid id)
        {
            try
            {
                var result = await _mediator.Send(new GetClinicalTrialByIdQuery(id));
                return Ok(result);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("status")]
        public async Task<ActionResult<IEnumerable<ClinicalTrialDto>>> GetByStatus([FromQuery] TrialStatus? status)
        {
            var result = await _mediator.Send(new GetTrialsByStatusQuery(status));
            return Ok(result);
        }

        [HttpPost("filter")]
        public async Task<ActionResult<IEnumerable<GetClinicalTrialDto>>> GetByFilter([FromBody] GetTrialsByFilterQuery query)
        {
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
