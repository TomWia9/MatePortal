using Application.YerbaMates.Queries.GetYerbaMate;
using MediatR;

namespace Application.YerbaMates.Commands.CreateYerbaMate
{
    public class CreateYerbaMateCommand : IRequest<YerbaMateDto>
    {
        public string Brand { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public string imgUrl { get; set; }
        public decimal AveragePrice { get; set; }
        public int NumberOfAddToFav { get; set; } = 0;
    }
}