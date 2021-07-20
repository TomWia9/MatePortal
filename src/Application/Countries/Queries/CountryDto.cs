using System;
using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Countries.Queries
{
    public class CountryDto : IMapFrom<Country>
    {
        public Guid Id { get; init; }
        public string Name { get; set; }
    }
}