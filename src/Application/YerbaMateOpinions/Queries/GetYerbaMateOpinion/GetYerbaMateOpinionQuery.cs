using System;
using MediatR;

namespace Application.YerbaMateOpinions.Queries.GetYerbaMateOpinion;

/// <summary>
///     Get single opinion about yerba mate query
/// </summary>
public class GetYerbaMateOpinionQuery : IRequest<YerbaMateOpinionDto>
{
    /// <summary>
    ///     Initializes GetYerbaMateOpinionQuery
    /// </summary>
    /// <param name="opinionId">Yerba mate opinion ID</param>
    public GetYerbaMateOpinionQuery(Guid opinionId)
    {
        OpinionId = opinionId;
    }

    /// <summary>
    ///     Yerba mate opinion ID
    /// </summary>
    public Guid OpinionId { get; }
}