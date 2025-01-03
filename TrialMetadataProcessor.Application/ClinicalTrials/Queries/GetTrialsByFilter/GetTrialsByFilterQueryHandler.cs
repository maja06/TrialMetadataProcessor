using AutoMapper;
using MediatR;
using TrialMetadataProcessor.Application.ClinicalTrials.DTOs;
using TrialMetadataProcessor.Application.Common.Extensions;
using TrialMetadataProcessor.Domain.Interfaces.UnitOfWork;

namespace TrialMetadataProcessor.Application.ClinicalTrials.Queries.GetTrialsByFilterQuery
{
    public class GetTrialsByFilterQueryHandler : IRequestHandler<GetTrialsByFilterQuery, IEnumerable<GetClinicalTrialDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetTrialsByFilterQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GetClinicalTrialDto>> Handle(
            GetTrialsByFilterQuery request,
            CancellationToken cancellationToken)
        {
            var query = (await _unitOfWork.ClinicalTrials.GetAllAsync()).AsQueryable();

            query = query
                .ApplyFilters(request.Filter)
                .ApplySearch(request.Search)
                .Skip(request.Skip)
                .Take(request.Take);

            var trials = query.ToList();
            return _mapper.Map<IEnumerable<GetClinicalTrialDto>>(trials);
        }
    }
}
