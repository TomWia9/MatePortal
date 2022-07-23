using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

namespace Application.Favourites.Queries.GetFavourites;

/// <summary>
///     Get user's favourites handler
/// </summary>
public class GetFavouritesHandler : IRequestHandler<GetFavouritesQuery, PaginatedList<FavouriteDto>>
{
    /// <summary>
    ///     Database context
    /// </summary>
    private readonly IApplicationDbContext _context;

    /// <summary>
    ///     The mapper
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    ///     Initializes GetFavouritesHandler
    /// </summary>
    /// <param name="context">Database context</param>
    /// <param name="mapper">The mapper</param>
    public GetFavouritesHandler(IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    ///     Handles getting user's favourites
    /// </summary>
    /// <param name="request">Get favourites request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of favourite data transfer objects</returns>
    /// <exception cref="ArgumentNullException">Thrown when parameters object is null</exception>
    public async Task<PaginatedList<FavouriteDto>> Handle(GetFavouritesQuery request,
        CancellationToken cancellationToken)
    {
        if (request.Parameters == null) throw new ArgumentNullException(nameof(request.Parameters));

        var collection = _context.Favourites.Where(f => f.CreatedBy == request.UserId);

        return await collection.OrderBy(f => f.YerbaMateId).ProjectTo<FavouriteDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Parameters.PageNumber, request.Parameters.PageSize);
    }
}