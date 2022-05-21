using System;
using MediatR;

namespace Application.Opinions.Commands.DeleteOpinion;

/// <summary>
///     Delete opinion command
/// </summary>
public class DeleteOpinionCommand : IRequest
{
    /// <summary>
    ///     Opinion ID
    /// </summary>
    public Guid OpinionId { get; init; }
}