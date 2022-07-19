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

namespace Application.YerbaMateImages.Queries.GetYerbaMateImages;

/// <summary>
/// Get yerba mate images handler
/// </summary>
public class GetYerbaMateImagesHandler : IRequestHandler<GetYerbaMateImagesQuery, PaginatedList<YerbaMateImageDto>>
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
    ///     Initializes GetYerbaMateImagesHandler
    /// </summary>
    /// <param name="context">Database context</param>
    /// <param name="mapper">The mapper</param>
    public GetYerbaMateImagesHandler(IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    ///     Handles getting yerba mate images
    /// </summary>
    /// <param name="request">Get yerba mate images request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Paginated list of yerba mate images data transfer objects</returns>
    /// <exception cref="ArgumentNullException">Thrown when parameters object is null</exception>
    public async Task<PaginatedList<YerbaMateImageDto>> Handle(GetYerbaMateImagesQuery request,
        CancellationToken cancellationToken)
    {
        if (request.Parameters == null) throw new ArgumentNullException(nameof(request.Parameters));

        var collection = _context.YerbaMateImages
            .Where(o => o.YerbaMateId == request.YerbaMateId).AsQueryable();

        //searching
        if (!string.IsNullOrWhiteSpace(request.Parameters.SearchQuery))
        {
            var searchQuery = request.Parameters.SearchQuery.Trim().ToLower();

            collection = collection.Where(o => o.Url.ToLower().Contains(searchQuery));
        }

        return await collection.ProjectTo<YerbaMateImageDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Parameters.PageNumber, request.Parameters.PageSize);
    }
}