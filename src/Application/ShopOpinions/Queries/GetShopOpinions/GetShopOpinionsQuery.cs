using Application.Common.Models;
using MediatR;

namespace Application.ShopOpinions.Queries.GetShopOpinions;

/// <summary>
///     Get shop opinions query
/// </summary>
public class GetShopOpinionsQuery : IRequest<PaginatedList<ShopOpinionDto>>
{
    /// <summary>
    ///     Initializes GetShopOpinionsQuery
    /// </summary>
    /// <param name="parameters">Shop opinions query parameters</param>
    public GetShopOpinionsQuery(ShopOpinionsQueryParameters parameters)
    {
        Parameters = parameters;
    }

    /// <summary>
    ///     Shop opinions query parameters
    /// </summary>
    public ShopOpinionsQueryParameters Parameters { get; }
}