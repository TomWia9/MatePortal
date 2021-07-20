using Application.YerbaMates.Queries;
using MediatR;

namespace Application.YerbaMates.Commands.CreateYerbaMate
{
    /// <summary>
    /// Create yerba mate command
    /// </summary>
    public class CreateYerbaMateCommand : IRequest<YerbaMateDto>
    {
        /// <summary>
        /// Yerba mate brand
        /// </summary>
        public string Brand { get; set; }

        /// <summary>
        /// Yerba mate name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Yerba mate description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Yerba mate category
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// Yerba mate image url
        /// </summary>
        public string imgUrl { get; set; }

        /// <summary>
        /// Yerba mate average price
        /// </summary>
        public decimal AveragePrice { get; set; }

        /// <summary>
        /// The number of additions of yerba to the favorites
        /// </summary>
        public int NumberOfAddToFav { get; set; } = 0;
    }
}