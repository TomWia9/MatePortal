using System;

namespace Application.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}