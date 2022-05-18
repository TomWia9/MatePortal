using System;
using MediatR;

namespace Application.Categories.Commands.DeleteCategory
{
    /// <summary>
    ///     Delete category command
    /// </summary>
    public class DeleteCategoryCommand : IRequest
    {
        /// <summary>
        ///     Category ID
        /// </summary>
        public Guid CategoryId { get; init; }
    }
}