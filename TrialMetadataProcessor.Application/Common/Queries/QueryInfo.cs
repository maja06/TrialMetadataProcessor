using TrialMetadataProcessor.Application.Common.Extensions;

namespace TrialMetadataProcessor.Application.Common.Queries
{
    public class QueryInfo
    {
        public FilterInfo Filter { get; set; }
        public SearchInfo Search { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }

        public async Task<List<TEntity>> ExecuteQueryAsync<TEntity>(IQueryable<TEntity> source)
        {
            source = source.ApplyFilters(Filter)
                           .ApplySearch(Search)
                           .ApplyPagination(Skip, Take);

            return source.ToList();
        }
    }
}
