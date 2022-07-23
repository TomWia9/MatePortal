using System;
using MediatR;

namespace Application.YerbaMateOpinions.Commands.DeleteYerbaMateOpinion;

/// <summary>
///     Delete yerba mate opinion command
/// </summary>
public class DeleteYerbaMateOpinionCommand : IRequest
{
    /// <summary>
    ///     Yerba mate opinion ID
    /// </summary>
    public Guid OpinionId { get; init; }
}