using System;
using MediatR;

namespace Application.Countries.Commands.UpdateCountry
{
    public class UpdateCountryCommand : IRequest
    {
        public Guid Id { get; init; }
        public string Name { get; set; }
    }
}