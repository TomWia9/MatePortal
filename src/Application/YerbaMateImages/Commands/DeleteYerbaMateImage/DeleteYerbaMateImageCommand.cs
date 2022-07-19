using System;
using MediatR;

namespace Application.YerbaMateImages.Commands.DeleteYerbaMateImage;

/// <summary>
///     Delete yerba mate image command
/// </summary>
public class DeleteYerbaMateImageCommand: IRequest
{
    /// <summary>
    ///     Yerba mate image ID
    /// </summary>
    public Guid YerbaMateImageId { get; init; }
}