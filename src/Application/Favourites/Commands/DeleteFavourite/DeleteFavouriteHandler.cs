using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Favourites.Commands.DeleteFavourite
{
    /// <summary>
    /// Delete favourite handler
    /// </summary>
    public class DeleteFavouriteHandler : IRequestHandler<DeleteFavouriteCommand>
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
        /// Initializes DeleteFavouriteHandler
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="currentUserService">Current user service</param>
        public DeleteFavouriteHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// Handles deleting favourite
        /// </summary>
        /// <param name="request">Delete favourite request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <exception cref="NotFoundException">Thrown when favourite is not found</exception>
        public async Task<Unit> Handle(DeleteFavouriteCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Favourites.FindAsync(request.FavouriteId);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Favourite), request.FavouriteId);
            }

            if (_currentUserService.UserRole != "Administrator" &&
                entity.CreatedBy != _currentUserService.UserId)
            {
                throw new ForbiddenAccessException();
            }

            _context.Favourites.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}