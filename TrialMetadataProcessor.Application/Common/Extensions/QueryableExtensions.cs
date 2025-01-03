using System.Linq.Expressions;
using TrialMetadataProcessor.Application.Common.Queries;

namespace TrialMetadataProcessor.Application.Common.Extensions
{
    public static class QueryableExtensions
    {    
        public static IQueryable<TEntity> ApplyFilters<TEntity>(this IQueryable<TEntity> source, FilterInfo filter)
        {
            if (filter == null || filter.Rules == null || !filter.Rules.Any())
                return source;

            var parameter = Expression.Parameter(typeof(TEntity), "x");
            var filterExpression = GetFilterExpression(parameter, filter.Rules.ToList(), filter.Condition);

            var lambda = Expression.Lambda<Func<TEntity, bool>>(filterExpression, parameter);
            return source.Where(lambda);
        }

        public static IQueryable<T> ApplySearch<T>(this IQueryable<T> query, SearchInfo search)
        {
            if (string.IsNullOrWhiteSpace(search?.SearchText) ||
                search.SearchProperties == null ||
                !search.SearchProperties.Any())
                return query;

            var parameter = Expression.Parameter(typeof(T), "x");
            var searchValue = search.SearchText.ToLower();
            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });


            Expression searchExpression = Expression.Constant(false);
            foreach (var property in search.SearchProperties)
            {
                var propertyExpression = Expression.Property(parameter, property);
                var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                var propertyToLower = Expression.Call(propertyExpression, toLowerMethod);
                var searchConstant = Expression.Constant(searchValue);

                var containsExpression = Expression.Call(propertyToLower, containsMethod, searchConstant);

                searchExpression = Expression.OrElse(searchExpression, containsExpression);
            }

            var lambda = Expression.Lambda<Func<T, bool>>(searchExpression, parameter);
            return query.Where(lambda);
        }

        public static IQueryable<TEntity> ApplyPagination<TEntity>(this IQueryable<TEntity> source, int skip, int take)
        {
            return source.Skip(skip).Take(take);
        }

        private static Expression GetFilterExpression(ParameterExpression parameter, List<RuleInfo> rules, FilterCondition condition)
        {
            Expression result = condition == FilterCondition.And ? Expression.Constant(true) : Expression.Constant(false);

            foreach (var rule in rules)
            {
                var propertyExpression = Expression.Property(parameter, rule.Property);
                object convertedValue;

                if (propertyExpression.Type.IsGenericType && propertyExpression.Type.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    var underlyingType = Nullable.GetUnderlyingType(propertyExpression.Type);
                    convertedValue = Convert.ChangeType(rule.Value, underlyingType);
                }
                else if (propertyExpression.Type.IsEnum)
                {
                    convertedValue = Enum.Parse(propertyExpression.Type, rule.Value);
                }
                else
                {
                    convertedValue = Convert.ChangeType(rule.Value, propertyExpression.Type);
                }

                var constant = Expression.Constant(convertedValue, propertyExpression.Type);

                BinaryExpression binaryExpression;
                switch (rule.Operator)
                {
                    case nameof(FilterOperator.Equals):
                        binaryExpression = Expression.Equal(propertyExpression, constant);
                        break;
                    case nameof(FilterOperator.GreaterThan):
                        binaryExpression = Expression.GreaterThan(propertyExpression, constant);
                        break;
                    case nameof(FilterOperator.LessThan):
                        binaryExpression = Expression.LessThan(propertyExpression, constant);
                        break;
                    case nameof(FilterOperator.Contains):
                        binaryExpression = Expression.Equal(BuildContainsExpression(propertyExpression, constant), Expression.Constant(true));
                        break;
                    case nameof(FilterOperator.StartsWith):
                        binaryExpression = Expression.Equal(BuildStartsWithExpression(propertyExpression, constant), Expression.Constant(true));
                        break;
                    case nameof(FilterOperator.EndsWith):
                        binaryExpression = Expression.Equal(BuildEndsWithExpression(propertyExpression, constant), Expression.Constant(true));
                        break;
                    default:
                        throw new InvalidOperationException($"Unexpected operator: {rule.Operator}");
                }

                result = condition == FilterCondition.And ? Expression.AndAlso(result, binaryExpression) : Expression.OrElse(result, binaryExpression);

                if (rule.Rules?.Any() == true)
                {
                    var nestedExpression = GetFilterExpression(parameter, rule.Rules.ToList(), rule.Condition ?? FilterCondition.And);
                    result = condition == FilterCondition.And ? Expression.AndAlso(result, nestedExpression) : Expression.OrElse(result, nestedExpression);
                }
            }

            return result;
        }

        private static Expression BuildContainsExpression(Expression property, Expression value)
        {
            var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
            return Expression.Call(property, containsMethod, value);
        }

        private static Expression BuildStartsWithExpression(Expression property, Expression value)
        {
            var startsWithMethod = typeof(string).GetMethod("StartsWith", new[] { typeof(string) });
            return Expression.Call(property, startsWithMethod, value);
        }

        private static Expression BuildEndsWithExpression(Expression property, Expression value)
        {
            var endsWithMethod = typeof(string).GetMethod("EndsWith", new[] { typeof(string) });
            return Expression.Call(property, endsWithMethod, value);
        }
    }
}

