using System;
using MediatR;

namespace Application.Countries.Commands.DeleteCountry
{
    public class DeleteCountryCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}