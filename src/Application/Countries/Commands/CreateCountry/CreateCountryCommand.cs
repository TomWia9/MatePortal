using Application.Countries.Queries;
using MediatR;

namespace Application.Countries.Commands.CreateCountry
{
    /// <summary>
    ///     Create country command
    /// </summary>
    public class CreateCountryCommand : IRequest<CountryDto>
    {
        /// <summary>
        ///     Country name
        /// </summary>
        public string Name { get; init; }
    }
}