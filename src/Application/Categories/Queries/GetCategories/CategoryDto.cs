using System;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.Categories.Queries.GetCategories
{
    public class CategoryDto : IMapFrom<Category>
    {
        public Guid Id { get; init; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}