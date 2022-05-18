using System;
using MediatR;

namespace Application.Countries.Commands.UpdateCountry
{
    /// <summary>
    ///     Update country command
    /// </summary>
    public class UpdateCountryCommand : IRequest
    {
        /// <summary>
        ///     Country ID
        /// </summary>
        public Guid CountryId { get; init; }

        /// <summary>
        ///     Country name
        /// </summary>
        public string Name { get; init; }
    }
}