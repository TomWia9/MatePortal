using System;
using MediatR;

namespace Application.Brands.Commands.UpdateBrand
{
    public class UpdateBrandCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Country { get; set; }
    }
}