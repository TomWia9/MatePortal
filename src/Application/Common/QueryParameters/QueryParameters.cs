using Application.Common.Enums;

namespace Application.Common.QueryParameters;

/// <summary>
///     Query parameters
/// </summary>
public abstract class QueryParameters
{
    private const int MaxPageSize = 30;
    private int _pageSize = 10;

    /// <summary>
    ///     The column by which to sort
    /// </summary>
    public string SortBy { get; init; }

    /// <summary>
    ///     The sort direction
    /// </summary>
    public SortDirection SortDirection { get; init; } = SortDirection.Asc;

    /// <summary>
    ///     The search query
    /// </summary>
    public string SearchQuery { get; init; }

    /// <summary>
    ///     The page number
    /// </summary>
    public int PageNumber { get; set; } = 1;

    /// <summary>
    ///     The page size
    /// </summary>
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }
}