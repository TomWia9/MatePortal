using Application.Common.QueryParameters;

namespace Application.ShopOpinions.Queries;

/// <summary>
///     Shop opinions query parameters
/// </summary>
public class ShopOpinionsQueryParameters : QueryParameters
{
    private readonly int _maxRate = 10;
    private readonly int _minRate = 1;

    /// <summary>
    ///     Minimum rate parameter
    /// </summary>
    public int MinRate
    {
        get => _minRate;
        init => _minRate = value is < 1 or > 10 ? 1 : value;
    }

    /// <summary>
    ///     Maximum rate parameter
    /// </summary>
    public int MaxRate
    {
        get => _maxRate;
        init => _maxRate = value is < 1 or > 10 ? 10 : value;
    }
}