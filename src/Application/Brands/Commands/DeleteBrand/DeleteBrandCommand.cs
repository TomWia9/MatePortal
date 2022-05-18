using System;
using MediatR;

namespace Application.Brands.Commands.DeleteBrand
{
    /// <summary>
    ///     Delete brand command
    /// </summary>
    public class DeleteBrandCommand : IRequest
    {
        /// <summary>
        ///     Brand ID
        /// </summary>
        public Guid BrandId { get; init; }
    }
}