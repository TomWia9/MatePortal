using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.YerbaMates.Commands.UpdateYerbaMate;

/// <summary>
///     Update yerba mate handler
/// </summary>
public class UpdateYerbaMateHandler : IRequestHandler<UpdateYerbaMateCommand>
{
    /// <summary>
    ///     Database context
    /// </summary>
    private readonly IApplicationDbContext _context;

    /// <summary>
    ///     Initializes UpdateYerbaMateHandler
    /// </summary>
    /// <param name="context">Database context</param>
    public UpdateYerbaMateHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    ///     Handles updating yerba mate
    /// </summary>
    /// <param name="request">Update yerba mate request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <exception cref="NotFoundException">Thrown when yerba mate, updated brand or category is not found</exception>
    /// <exception cref="ConflictException">Thrown when yerba mate name conflicts with another yerba mate name</exception>
    public async Task<Unit> Handle(UpdateYerbaMateCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.YerbaMate.FindAsync(request.YerbaMateId);

        if (entity == null) throw new NotFoundException(nameof(YerbaMate), request.YerbaMateId);

        if (!await _context.Brands.AnyAsync(b => b.Id == request.BrandId, cancellationToken))
            throw new NotFoundException(nameof(Brand), request.BrandId);

        if (!await _context.Categories.AnyAsync(c => c.Id == request.CategoryId,
                cancellationToken))
            throw new NotFoundException(nameof(Category), request.CategoryId);

        if (await _context.YerbaMate.Where(x => x != entity).AnyAsync(x => x.Name == request.Name,
                cancellationToken))
            throw new ConflictException();

        entity.Name = request.Name;
        entity.Description = request.Description;
        entity.AveragePrice = request.AveragePrice;
        entity.BrandId = request.BrandId;
        entity.CategoryId = request.CategoryId;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}