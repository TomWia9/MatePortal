using System;
using Domain.Entities;

namespace Application.Brands.Queries.GetBrands
{
    public class BrandDto
    {
        public Guid Id { get; init; }
        public string Name { get; set; }
        public string Description { get; set; }
        public CountryDto Country { get; set; }
    }
}