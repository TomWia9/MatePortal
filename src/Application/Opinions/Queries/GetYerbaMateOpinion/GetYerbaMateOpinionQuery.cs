using System;
using MediatR;

namespace Application.Opinions.Queries.GetYerbaMateOpinion;

/// <summary>
///     Get single opinion about yerba mate query
/// </summary>
public class GetYerbaMateOpinionQuery : IRequest<OpinionDto>
{
    /// <summary>
    ///     Initializes GetYerbaMateOpinionQuery
    /// </summary>
    /// <param name="opinionId">Opinion ID</param>
    public GetYerbaMateOpinionQuery(Guid opinionId)
    {
        OpinionId = opinionId;
    }

    /// <summary>
    ///     Opinion ID
    /// </summary>
    public Guid OpinionId { get; }
}