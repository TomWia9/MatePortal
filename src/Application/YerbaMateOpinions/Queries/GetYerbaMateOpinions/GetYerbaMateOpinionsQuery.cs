using System;
using Application.Common.Models;
using MediatR;

namespace Application.YerbaMateOpinions.Queries.GetYerbaMateOpinions;

/// <summary>
///     Get all opinions about single yerba mate query
/// </summary>
public class GetYerbaMateOpinionsQuery : IRequest<PaginatedList<YerbaMateOpinionDto>>
{
    /// <summary>
    ///     Initializes GetYerbaMateOpinionsQuery
    /// </summary>
    /// <param name="yerbaMateId">YerbaMate ID from which opinions can be obtained</param>
    /// <param name="parameters">Yerba mate opinions query parameters</param>
    public GetYerbaMateOpinionsQuery(Guid yerbaMateId, YerbaMateOpinionsQueryParameters parameters)
    {
        YerbaMateId = yerbaMateId;
        Parameters = parameters;
    }

    /// <summary>
    ///     YerbaMate ID from which opinions can be obtained
    /// </summary>
    public Guid YerbaMateId { get; }

    /// <summary>
    ///     Yerba mate opinions query parameters
    /// </summary>
    public YerbaMateOpinionsQueryParameters Parameters { get; }
}