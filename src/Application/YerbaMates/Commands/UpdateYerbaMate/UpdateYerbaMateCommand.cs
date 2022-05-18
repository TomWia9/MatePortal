using System;
using MediatR;

namespace Application.YerbaMates.Commands.UpdateYerbaMate
{
    /// <summary>
    ///     Update yerba mate command
    /// </summary>
    public class UpdateYerbaMateCommand : IRequest
    {
        /// <summary>
        ///     Yerba mate ID
        /// </summary>
        public Guid YerbaMateId { get; init; }

        /// <summary>
        ///     Yerba mate name
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        ///     Yerba mate description
        /// </summary>
        public string Description { get; init; }

        /// <summary>
        ///     Yerba mate image url
        /// </summary>
        public string imgUrl { get; init; }

        /// <summary>
        ///     Yerba mate average price
        /// </summary>
        public decimal AveragePrice { get; init; }

        /// <summary>
        ///     Yerba mate brand ID
        /// </summary>
        public Guid BrandId { get; init; }

        /// <summary>
        ///     Yerba mate category ID
        /// </summary>
        public Guid CategoryId { get; init; }
    }
}