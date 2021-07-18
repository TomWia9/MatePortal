using Application.Categories.Queries.GetCategories;
using MediatR;

namespace Application.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommand : IRequest<CategoryDto>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}