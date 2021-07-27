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
        /// Yerba mate service
        /// </summary>
        private readonly IYerbaMateService _yerbaMateService;

        /// <summary>
        /// Initializes DeleteFavouriteHandler
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="yerbaMateService">Yerba mate service</param>
        public DeleteFavouriteHandler(IApplicationDbContext context, IYerbaMateService yerbaMateService)
        {
            _context = context;
            _yerbaMateService = yerbaMateService;
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
            
            await _yerbaMateService.DecreaseNumberOfAddToFav(entity.YerbaMateId, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}