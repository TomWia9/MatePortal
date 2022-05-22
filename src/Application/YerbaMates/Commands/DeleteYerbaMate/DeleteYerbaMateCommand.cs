using System;
using MediatR;

namespace Application.YerbaMates.Commands.DeleteYerbaMate;

/// <summary>
///     Delete yerba mate command
/// </summary>
public class DeleteYerbaMateCommand : IRequest
{
    /// <summary>
    ///     Yerba mate ID
    /// </summary>
    public Guid YerbaMateId { get; init; }
}