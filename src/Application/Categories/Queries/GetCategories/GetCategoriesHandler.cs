using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using Application.Common.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;

namespace Application.Categories.Queries.GetCategories;

/// <summary>
///     Get categories handler
/// </summary>
public class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, PaginatedList<CategoryDto>>
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
    ///     Initializes GetCategoriesHandler
    /// </summary>
    /// <param name="context">Database context</param>
    /// <param name="mapper">The mapper</param>
    public GetCategoriesHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    ///     Handles getting categories
    /// </summary>
    /// <param name="request">Get categories request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of category data transfer objects</returns>
    public async Task<PaginatedList<CategoryDto>> Handle(GetCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var categories = _context.Categories.AsQueryable();
        
        //TODO Add searching and sorting

        return await categories.OrderBy(x => x.Name).ProjectTo<CategoryDto>(_mapper.ConfigurationProvider)
            .PaginatedListAsync(request.Parameters.PageNumber, request.Parameters.PageSize);
    }
}