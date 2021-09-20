using System;
using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Categories.Queries
{
    /// <summary>
    ///     Category data transfer object
    /// </summary>
    public class CategoryDto : IMapFrom<Category>
    {
        /// <summary>
        ///     Category ID
        /// </summary>
        public Guid Id { get; init; }

        /// <summary>
        ///     Category name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     Category description
        /// </summary>
        public string Description { get; set; }
    }
}