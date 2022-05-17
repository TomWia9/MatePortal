using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Shops.Commands.DeleteShop
{
    /// <summary>
    /// Delete shop handler
    /// </summary>
    public class DeleteShopHandler : IRequestHandler<DeleteShopCommand>
    {
        /// <summary>
        /// Database context
        /// </summary>
        private readonly IApplicationDbContext _context;

        /// <summary>
        /// Initializes DeleteShopHandler
        /// </summary>
        /// <param name="context">Database context</param>
        public DeleteShopHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Handles deleting shop
        /// </summary>
        /// <param name="request">Delete shop request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <exception cref="NotFoundException">Thrown when shop is not found</exception>
        public async Task<Unit> Handle(DeleteShopCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Shops.FindAsync(request.ShopId);

            if (entity == null) throw new NotFoundException(nameof(Shop), request.ShopId);

            _context.Shops.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}