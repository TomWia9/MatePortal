using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Categories.Commands.DeleteCategory;

/// <summary>
///     Delete category handler
/// </summary>
public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand>
{
    /// <summary>
    ///     Database context
    /// </summary>
    private readonly IApplicationDbContext _context;

    /// <summary>
    ///     Initializes DeleteCategoryHandler
    /// </summary>
    /// <param name="context">Database context</param>
    public DeleteCategoryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    ///     Handles deleting category
    /// </summary>
    /// <param name="request">Delete category request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <exception cref="NotFoundException">Thrown when category is not found</exception>
    public async Task<Unit> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Categories.FindAsync(request.CategoryId);

        if (entity == null) throw new NotFoundException(nameof(Category), request.CategoryId);

        _context.Categories.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}