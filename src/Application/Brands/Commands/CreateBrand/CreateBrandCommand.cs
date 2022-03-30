using System;
using Application.Brands.Queries;
using MediatR;

namespace Application.Brands.Commands.CreateBrand
{
    /// <summary>
    /// Create brand command
    /// </summary>
    public class CreateBrandCommand : IRequest<BrandDto>
    {
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