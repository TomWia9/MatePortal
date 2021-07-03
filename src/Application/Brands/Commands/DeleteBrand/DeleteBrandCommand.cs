using System;
using MediatR;

namespace Application.Brands.Commands.DeleteBrand
{
    public class DeleteBrandCommand : IRequest
    {
        public Guid Id { get; set; }
    }
}