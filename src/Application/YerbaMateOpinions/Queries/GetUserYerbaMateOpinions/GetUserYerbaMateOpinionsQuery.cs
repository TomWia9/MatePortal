using System;
using Application.Common.Models;
using MediatR;

namespace Application.YerbaMateOpinions.Queries.GetUserYerbaMateOpinions;

/// <summary>
///     Get all opinions about yerba mate posted by single user
/// </summary>
public class GetUserYerbaMateOpinionsQuery : IRequest<PaginatedList<YerbaMateOpinionDto>>
{
    /// <summary>
    ///     Initializes GetUserYerbaMateOpinionsQuery
    /// </summary>
    /// <param name="userId">The user ID</param>
    /// <param name="parameters">Opinions query parameters</param>
    public GetUserYerbaMateOpinionsQuery(Guid userId, YerbaMateOpinionsQueryParameters parameters)
    {
        UserId = userId;
        Parameters = parameters;
    }

    /// <summary>
    ///     User ID
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    ///     Opinions query parameters
    /// </summary>
    public YerbaMateOpinionsQueryParameters Parameters { get; }
}