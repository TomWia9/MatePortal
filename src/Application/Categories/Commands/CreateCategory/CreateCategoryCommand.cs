using Application.Categories.Queries;
using MediatR;

namespace Application.Categories.Commands.CreateCategory
{
    /// <summary>
    ///     Create category command
    /// </summary>
    public class CreateCategoryCommand : IRequest<CategoryDto>
    {
        /// <summary>
        ///     Category name
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        ///     Category description
        /// </summary>
        public string Description { get; init; }
    }
}