using System;
using MediatR;

namespace Application.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}