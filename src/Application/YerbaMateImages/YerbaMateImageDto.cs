using System;
using Application.Common.Mappings;
using Domain.Entities;

namespace Application.YerbaMateImages;

/// <summary>
///     Yerba mate image data transfer object
/// </summary>
public class YerbaMateImageDto : IMapFrom<YerbaMateImage>
{
    /// <summary>
    ///     Yerba mate image ID
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    ///     Yerba mate image url
    /// </summary>
    public string Url { get; init; }
    
    /// <summary>
    ///     Yerba mate image created date
    /// </summary>
    public DateTime Created { get; set; }

    /// <summary>
    ///     Yerba mate ID
    /// </summary>
    public Guid YerbaMateId { get; set; }

    /// <summary>
    ///     User ID
    /// </summary>
    public Guid CreatedBy { get; set; }
}