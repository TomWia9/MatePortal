using System;
using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Favourites.Queries;

/// <summary>
///     Favourite data transfer object
/// </summary>
public class FavouriteDto : IMapFrom<Favourite>
{
    /// <summary>
    ///     Favourite ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///     Yerba mate ID
    /// </summary>
    public Guid YerbaMateId { get; set; }
}