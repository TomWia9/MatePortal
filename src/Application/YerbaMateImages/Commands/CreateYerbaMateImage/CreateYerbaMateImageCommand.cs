using System;
using MediatR;

namespace Application.YerbaMateImages.Commands.CreateYerbaMateImage;

/// <summary>
///     Create yerba mate image command
/// </summary>
public class CreateYerbaMateImageCommand: IRequest<YerbaMateImageDto>
{
    /// <summary>
    ///     Yerba mate image url
    /// </summary>
    public string Url { get; init; }

    /// <summary>
    ///     Yerba mate ID
    /// </summary>
    public Guid YerbaMateId { get; init; }
}