using System;

namespace Application.Categories.Queries.GetCategories
{
    public class CategoryDto
    {
        public Guid Id { get; init; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}