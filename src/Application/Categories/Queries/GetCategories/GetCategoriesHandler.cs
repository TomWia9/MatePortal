using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using AutoMapper;
using MediatR;

namespace Application.Categories.Queries.GetCategories;

/// <summary>
///     Get categories handler
/// </summary>
public class GetCategoriesHandler : IRequestHandler<GetCategoriesQuery, IEnumerable<CategoryDto>>
{
    /// <summary>
    ///     Database context
    /// </summary>
    private readonly IApplicationDbContext _context;

    /// <summary>
    ///     Initializes GetCategoriesHandler
    /// </summary>
    /// <param name="context">Database context</param>
    public GetCategoriesHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    ///     Handles getting categories
    /// </summary>
    /// <param name="request">Get categories request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>List of category data transfer objects</returns>
    public async Task<IEnumerable<CategoryDto>> Handle(GetCategoriesQuery request,
        CancellationToken cancellationToken)
    {
        var categories = _context.Categories.AsQueryable();

        var mapperConfiguration = new MapperConfiguration(cfg => { cfg.AddProfile<MappingProfile>(); });

        return await categories.ProjectToListAsync<CategoryDto>(mapperConfiguration);
    }
}