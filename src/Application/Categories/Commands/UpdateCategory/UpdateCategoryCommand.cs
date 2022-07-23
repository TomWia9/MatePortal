using System;
using MediatR;

namespace Application.Categories.Commands.UpdateCategory;

/// <summary>
///     Update category command
/// </summary>
public class UpdateCategoryCommand : IRequest
{
    /// <summary>
    ///     Category ID
    /// </summary>
    public Guid CategoryId { get; init; }

    /// <summary>
    ///     Category name
    /// </summary>
    public string Name { get; init; }

    /// <summary>
    ///     Category description
    /// </summary>
    public string Description { get; init; }
}