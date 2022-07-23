using System;
using MediatR;

namespace Application.YerbaMateOpinions.Commands.UpdateYerbaMateOpinion;

/// <summary>
///     Update yerba mate opinion command
/// </summary>
public class UpdateYerbaMateOpinionCommand : IRequest
{
    /// <summary>
    ///     Yerba mate opinion ID
    /// </summary>
    public Guid OpinionId { get; init; }

    /// <summary>
    ///     Yerba mate opinion rate
    /// </summary>
    public int Rate { get; init; }

    /// <summary>
    ///     Yerba mate opinion comment
    /// </summary>
    public string Comment { get; init; }
}