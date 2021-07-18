using Domain.Entities;
using MediatR;

namespace Application.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommand : IRequest<Category>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}