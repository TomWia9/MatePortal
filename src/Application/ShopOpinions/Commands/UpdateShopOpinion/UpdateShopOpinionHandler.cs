using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.ShopOpinions.Commands.UpdateShopOpinion
{
    /// <summary>
    /// Update shop opinion handler
    /// </summary>
    public class UpdateShopOpinionHandler : IRequestHandler<UpdateShopOpinionCommand>
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
        /// Initializes UpdateShopOpinionHandler
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="currentUserService">Current user service</param>
        public UpdateShopOpinionHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// Handles updating shop opinion
        /// </summary>
        /// <param name="request">Update shop opinion request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <exception cref="NotFoundException">Thrown when shop opinion is not found</exception>
        /// <exception cref="ForbiddenAccessException">Thrown when user doesn't have access to shop opinion</exception>
        public async Task<Unit> Handle(UpdateShopOpinionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.ShopOpinions.FindAsync(request.ShopOpinionId);

            if (entity == null) throw new NotFoundException(nameof(ShopOpinion), request.ShopOpinionId);

            var currentUserId = _currentUserService.UserId;
            if (entity.CreatedBy != currentUserId && _currentUserService.UserRole != "Administrator")
                throw new ForbiddenAccessException();

            entity.Rate = request.Rate;
            entity.Comment = request.Comment;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}