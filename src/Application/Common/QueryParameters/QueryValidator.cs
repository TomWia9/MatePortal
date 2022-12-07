using FluentValidation;

namespace Application.Common.QueryParameters;

/// <summary>
///     Query validator
/// </summary>
public class QueryValidator<T> : AbstractValidator<T> where T : QueryParameters
{
    /// <summary>
    ///     Initializes QueryValidator
    /// </summary>
    public QueryValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).GreaterThanOrEqualTo(1);
        RuleFor(x => x.SearchQuery).MaximumLength(100);
        RuleFor(x => x.SortDirection).IsInEnum();
    }
}