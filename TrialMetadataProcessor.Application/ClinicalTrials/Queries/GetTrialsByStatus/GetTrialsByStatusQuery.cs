using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrialMetadataProcessor.Application.ClinicalTrials.DTOs;
using TrialMetadataProcessor.Domain.Entities.Enums;

namespace TrialMetadataProcessor.Application.ClinicalTrials.Queries.GetTrialsByStatus
{
    public record GetTrialsByStatusQuery(TrialStatus? Status) : IRequest<IEnumerable<GetClinicalTrialDto>>;
}
