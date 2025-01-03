using AutoMapper;
using TrialMetadataProcessor.Application.ClinicalTrials.DTOs;
using TrialMetadataProcessor.Domain.Entities.ClinicalTrial;

namespace TrialMetadataProcessor.Application.Common.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ClinicalTrial, GetClinicalTrialDto>();
        }
    }
}
