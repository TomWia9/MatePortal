using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Shops.Commands.UpdateShop
{
    /// <summary>
    /// Update shop handler
    /// </summary>
    public class UpdateShopHandler : IRequestHandler<UpdateShopCommand>
    {
        /// <summary>
        /// Database context
        /// </summary>
        private readonly IApplicationDbContext _context;

        /// <summary>
        /// Initializes UpdateShopHandler
        /// </summary>
        /// <param name="context">Database context</param>
        public UpdateShopHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Handles updating shop
        /// </summary>
        /// <param name="request">Update shop request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <exception cref="NotFoundException">Thrown when shop is not found</exception>
        /// <exception cref="ConflictException">Thrown when shop name conflicts with another shop name</exception>
        public async Task<Unit> Handle(UpdateShopCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Shops.FindAsync(request.ShopId);

            if (entity == null) throw new NotFoundException(nameof(Shop), request.ShopId);

            if (await _context.Shops.AnyAsync(b => b.Name == request.Name && entity.Name == request.Name,
                cancellationToken))
                throw new ConflictException();

            entity.Name = request.Name;
            entity.Description = request.Description;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}