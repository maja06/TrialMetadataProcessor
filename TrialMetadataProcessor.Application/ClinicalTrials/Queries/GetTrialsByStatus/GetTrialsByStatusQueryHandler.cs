using AutoMapper;
using MediatR;
using TrialMetadataProcessor.Application.ClinicalTrials.DTOs;
using TrialMetadataProcessor.Domain.Interfaces.UnitOfWork;

namespace TrialMetadataProcessor.Application.ClinicalTrials.Queries.GetTrialsByStatus
{
    public class GetTrialsByStatusQueryHandler : IRequestHandler<GetTrialsByStatusQuery, IEnumerable<GetClinicalTrialDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetTrialsByStatusQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetClinicalTrialDto>> Handle(
            GetTrialsByStatusQuery request,
            CancellationToken cancellationToken)
        {
            var trials = request.Status.HasValue
                ? await _unitOfWork.ClinicalTrials.GetByStatusAsync(request.Status.Value)
                : await _unitOfWork.ClinicalTrials.GetAllAsync();

            return _mapper.Map<IEnumerable<GetClinicalTrialDto>>(trials);
        }
    }
}
