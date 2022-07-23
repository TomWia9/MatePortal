using System;
using Application.Common.Models;
using MediatR;

namespace Application.YerbaMateImages.Queries.GetYerbaMateImages;

/// <summary>
///     Get yerba mate images query
/// </summary>
public class GetYerbaMateImagesQuery: IRequest<PaginatedList<YerbaMateImageDto>>
{
    /// <summary>
    ///     Initializes GetYerbaMateImagesQuery
    /// </summary>
    /// <param name="yerbaMateId">YerbaMate ID from which images can be obtained</param>
    /// <param name="parameters">Yerba mate images query parameters</param>
    public GetYerbaMateImagesQuery(Guid yerbaMateId, YerbaMateImagesQueryParameters parameters)
    {
        YerbaMateId = yerbaMateId;
        Parameters = parameters;
    }

    /// <summary>
    ///     YerbaMate ID from which images can be obtained
    /// </summary>
    public Guid YerbaMateId { get; }

    /// <summary>
    ///     Yerba mate images query parameters
    /// </summary>
    public YerbaMateImagesQueryParameters Parameters { get; }
}