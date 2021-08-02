using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Opinions.Commands.DeleteOpinion
{
    /// <summary>
    /// Delete yerba mate opinion handler
    /// </summary>
    public class DeleteOpinionHandler : IRequestHandler<DeleteOpinionCommand>
    {
        /// <summary>
        /// Database context
        /// </summary>
        private readonly IApplicationDbContext _context;

        /// <summary>
        /// Current user service
        /// </summary>
        private readonly ICurrentUserService _currentUserService;

        /// <summary>
        /// Initializes DeleteOpinionHandler
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="currentUserService">Current user service</param>
        public DeleteOpinionHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// Handles deleting yerba mate opinion
        /// </summary>
        /// <param name="request">Delete yerba mate opinion request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <exception cref="NotFoundException">Thrown when yerba mate opinion is not found</exception>
        public async Task<Unit> Handle(DeleteOpinionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Opinions.FindAsync(request.OpinionId);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Opinion), request.OpinionId);
            }

            if (await _currentUserService.GetCurrentUserRoleAsync() != "Administrator" &&
                entity.CreatedBy != _currentUserService.UserId)
            {
                throw new ForbiddenAccessException();
            }

            _context.Opinions.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}