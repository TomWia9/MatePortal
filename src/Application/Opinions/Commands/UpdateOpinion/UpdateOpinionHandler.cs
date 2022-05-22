using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Opinions.Commands.UpdateOpinion;

/// <summary>
///     Update yerba mate opinion handler
/// </summary>
public class UpdateYerbaMateOpinionHandler : IRequestHandler<UpdateOpinionCommand>
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
    ///     Initializes UpdateYerbaMateOpinionHandler
    /// </summary>
    /// <param name="context">Database context</param>
    /// <param name="currentUserService">Current user service</param>
    public UpdateYerbaMateOpinionHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    /// <summary>
    ///     Handles updating yerba mate opinion
    /// </summary>
    /// <param name="request">Update yerba mate opinion request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <exception cref="NotFoundException">Thrown when opinion is not found</exception>
    /// <exception cref="ForbiddenAccessException">Thrown when user doesn't have access to opinion</exception>
    public async Task<Unit> Handle(UpdateOpinionCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Opinions.FindAsync(request.OpinionId);

        if (entity == null) throw new NotFoundException(nameof(Opinion), request.OpinionId);

        var currentUserId = _currentUserService.UserId;
        if (entity.CreatedBy != currentUserId && !_currentUserService.AdministratorAccess)
            throw new ForbiddenAccessException();

        entity.Rate = request.Rate;
        entity.Comment = request.Comment;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}