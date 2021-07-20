using System;
using MediatR;

namespace Application.Countries.Commands.DeleteCountry
{
    /// <summary>
    /// Delete country command
    /// </summary>
    public class DeleteCountryCommand : IRequest
    {
        /// <summary>
        /// Country ID
        /// </summary>
        public Guid CountryId { get; init; }
    }
}