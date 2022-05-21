using System;
using Application.Opinions.Queries;
using MediatR;

namespace Application.Opinions.Commands.CreateOpinion;

/// <summary>
///     Create opinion command
/// </summary>
public class CreateOpinionCommand : IRequest<OpinionDto>
{
    /// <summary>
    ///     Opinion rate
    /// </summary>
    public int Rate { get; init; }

    /// <summary>
    ///     Opinion comment
    /// </summary>
    public string Comment { get; init; }

    /// <summary>
    ///     Yerba mate ID
    /// </summary>
    public Guid YerbaMateId { get; init; }
}