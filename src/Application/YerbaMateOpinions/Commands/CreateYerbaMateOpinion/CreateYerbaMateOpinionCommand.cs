using System;
using Application.YerbaMateOpinions.Queries;
using MediatR;

namespace Application.YerbaMateOpinions.Commands.CreateYerbaMateOpinion;

/// <summary>
///     Create yerba mate opinion command
/// </summary>
public class CreateYerbaMateOpinionCommand : IRequest<YerbaMateOpinionDto>
{
    /// <summary>
    ///     Yerba mate opinion rate
    /// </summary>
    public int Rate { get; init; }

    /// <summary>
    ///     Yerba mate opinion comment
    /// </summary>
    public string Comment { get; init; }

    /// <summary>
    ///     Yerba mate ID
    /// </summary>
    public Guid YerbaMateId { get; init; }
}