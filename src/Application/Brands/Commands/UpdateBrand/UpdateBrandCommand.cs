using System;
using MediatR;

namespace Application.Brands.Commands.UpdateBrand
{
    /// <summary>
    ///     Update brand command
    /// </summary>
    public class UpdateBrandCommand : IRequest
    {
        /// <summary>
        ///     Brand ID
        /// </summary>
        public Guid BrandId { get; init; }

        /// <summary>
        ///     Brand name
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        ///     Brand description
        /// </summary>
        public string Description { get; init; }

        /// <summary>
        ///     Brand country ID
        /// </summary>
        public Guid CountryId { get; init; }
    }
}