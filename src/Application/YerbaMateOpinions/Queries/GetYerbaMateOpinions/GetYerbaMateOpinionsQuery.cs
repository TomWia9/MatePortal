using Application.Common.Models;
using MediatR;

namespace Application.YerbaMateOpinions.Queries.GetYerbaMateOpinions;

/// <summary>
///     Get yerba mate opinions query
/// </summary>
public class GetYerbaMateOpinionsQuery : IRequest<PaginatedList<YerbaMateOpinionDto>>
{
    /// <summary>
    ///     Initializes GetYerbaMateOpinionsQuery
    /// </summary>
    /// <param name="parameters">Yerba mate opinions query parameters</param>
    public GetYerbaMateOpinionsQuery(YerbaMateOpinionsQueryParameters parameters)
    {
        Parameters = parameters;
    }

    /// <summary>
    ///     Yerba mate opinions query parameters
    /// </summary>
    public YerbaMateOpinionsQueryParameters Parameters { get; }
}