using System;
using Application.Common.Models;
using MediatR;

namespace Application.Favourites.Queries.GetFavourites;

/// <summary>
///     Get user favourites yerba mates query
/// </summary>
public class GetFavouritesQuery : IRequest<PaginatedList<FavouriteDto>>
{
    /// <summary>
    ///     Initializes GetFavouritesQuery
    /// </summary>
    /// <param name="userId">User ID</param>
    /// <param name="parameters">Favourites query parameters</param>
    public GetFavouritesQuery(Guid userId, FavouritesQueryParameters parameters)
    {
        UserId = userId;
        Parameters = parameters;
    }

    /// <summary>
    ///     User ID
    /// </summary>
    public Guid UserId { get; }

    /// <summary>
    ///     Favourites query parameters
    /// </summary>
    public FavouritesQueryParameters Parameters { get; }
}