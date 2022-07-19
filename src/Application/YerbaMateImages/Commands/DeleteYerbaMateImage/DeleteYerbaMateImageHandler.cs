using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.YerbaMateImages.Commands.DeleteYerbaMateImage;

public class DeleteYerbaMateImageHandler : IRequestHandler<DeleteYerbaMateImageCommand>
{
    /// <summary>
    ///     Database context
    /// </summary>
    private readonly IApplicationDbContext _context;

    /// <summary>
    ///     Initializes DeleteOpinionHandler
    /// </summary>
    /// <param name="context">Database context</param>
    public DeleteYerbaMateImageHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    ///     Handles deleting yerba mate image
    /// </summary>
    /// <param name="request">Delete yerba mate image request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <exception cref="NotFoundException">Thrown when yerba mate image is not found</exception>
    public async Task<Unit> Handle(DeleteYerbaMateImageCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.YerbaMateImages.FindAsync(request.YerbaMateImageId);

        if (entity == null) throw new NotFoundException(nameof(YerbaMateImage), request.YerbaMateImageId);

        _context.YerbaMateImages.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}