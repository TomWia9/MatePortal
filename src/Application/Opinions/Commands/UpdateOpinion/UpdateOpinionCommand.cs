using System;
using MediatR;

namespace Application.Opinions.Commands.UpdateOpinion;

/// <summary>
///     Update opinion command
/// </summary>
public class UpdateOpinionCommand : IRequest
{
    /// <summary>
    ///     Opinion ID
    /// </summary>
    public Guid OpinionId { get; init; }

    /// <summary>
    ///     Opinion rate
    /// </summary>
    public int Rate { get; init; }

    /// <summary>
    ///     Opinion comment
    /// </summary>
    public string Comment { get; init; }
}