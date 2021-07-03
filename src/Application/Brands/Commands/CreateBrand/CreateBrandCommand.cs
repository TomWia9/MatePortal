using MediatR;

namespace Application.Brands.Commands.CreateBrand
{
    public class CreateBrandCommand : IRequest<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Country { get; set; }
    }
}