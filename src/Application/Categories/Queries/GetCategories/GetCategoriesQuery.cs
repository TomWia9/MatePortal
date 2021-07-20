using System.Collections.Generic;
using MediatR;

namespace Application.Categories.Queries.GetCategories
{
    /// <summary>
    /// Get all categories query
    /// </summary>
    public class GetCategoriesQuery : IRequest<IEnumerable<CategoryDto>>
    {
    }
}