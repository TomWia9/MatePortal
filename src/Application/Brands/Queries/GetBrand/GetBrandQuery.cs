using System;
using MediatR;

namespace Application.Brands.Queries.GetBrand;

/// <summary>
///     Get single brand query
/// </summary>
public class GetBrandQuery : IRequest<BrandDto>
{
    /// <summary>
    ///     Initializes GetBrandQuery
    /// </summary>
    /// <param name="brandId">Brand ID</param>
    public GetBrandQuery(Guid brandId)
    {
        BrandId = brandId;
    }

    /// <summary>
    ///     Brand ID
    /// </summary>
    public Guid BrandId { get; }
}