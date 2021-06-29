using System;

namespace Application.Countries.Queries.GetCountries
{
    public class CountryDto
    {
        public Guid Id { get; init; }
        public string Name { get; set; }
    }
}