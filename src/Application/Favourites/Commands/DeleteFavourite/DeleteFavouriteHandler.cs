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
        /// Initializes DeleteFavouriteHandler
        /// </summary>
        /// <param name="context">Database context</param>
        public DeleteFavouriteHandler(IApplicationDbContext context)
        {
            _context = context;
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
                throw new NotFoundException(nameof(Brand), request.FavouriteId);
            }

            _context.Favourites.Remove(entity);
            
            //TODO Decrease yerba yerba mate numOfAddToFav

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}