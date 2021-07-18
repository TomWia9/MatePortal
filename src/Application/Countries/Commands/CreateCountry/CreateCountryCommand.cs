using Application.Countries.Queries.GetCountries;
using MediatR;

namespace Application.Countries.Commands.CreateCountry
{
    public class CreateCountryCommand : IRequest<CountryDto>
    {
        public string Name { get; set; }
    }
}