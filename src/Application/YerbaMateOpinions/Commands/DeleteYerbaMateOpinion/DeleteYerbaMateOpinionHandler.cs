using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.YerbaMateOpinions.Commands.DeleteYerbaMateOpinion;

/// <summary>
///     Delete yerba mate opinion handler
/// </summary>
public class DeleteYerbaMateOpinionHandler : IRequestHandler<DeleteYerbaMateOpinionCommand>
{
    /// <summary>
    ///     Database context
    /// </summary>
    private readonly IApplicationDbContext _context;

    /// <summary>
    ///     Current user service
    /// </summary>
    private readonly ICurrentUserService _currentUserService;

    /// <summary>
    ///     Initializes DeleteOpinionHandler
    /// </summary>
    /// <param name="context">Database context</param>
    /// <param name="currentUserService">Current user service</param>
    public DeleteYerbaMateOpinionHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    /// <summary>
    ///     Handles deleting yerba mate opinion
    /// </summary>
    /// <param name="request">Delete yerba mate opinion request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <exception cref="NotFoundException">Thrown when yerba mate opinion is not found</exception>
    public async Task<Unit> Handle(DeleteYerbaMateOpinionCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.YerbaMateOpinions.FindAsync(request.OpinionId);

        if (entity == null) throw new NotFoundException(nameof(YerbaMateOpinion), request.OpinionId);

        if (!_currentUserService.AdministratorAccess &&
            entity.CreatedBy != _currentUserService.UserId)
            throw new ForbiddenAccessException();

        _context.YerbaMateOpinions.Remove(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}