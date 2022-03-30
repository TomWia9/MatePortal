using System;
using MediatR;

namespace Application.Brands.Commands.UpdateBrand
{
    /// <summary>
    /// Update brand command
    /// </summary>
    public class UpdateBrandCommand : IRequest
    {
        /// <summary>
        /// Brand ID
        /// </summary>
        public Guid BrandId { get; set; }

        /// <summary>
        /// Brand name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Brand description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Brand country ID
        /// </summary>
        public Guid CountryId { get; set; }
    }
}