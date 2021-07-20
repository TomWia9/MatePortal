using Application.Brands.Queries;
using MediatR;

namespace Application.Brands.Commands.CreateBrand
{
    public class CreateBrandCommand : IRequest<BrandDto>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Country { get; set; }
    }
}