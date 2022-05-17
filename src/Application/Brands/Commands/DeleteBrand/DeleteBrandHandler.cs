using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Brands.Commands.DeleteBrand
{
    /// <summary>
    /// Delete brand handler
    /// </summary>
    public class DeleteBrandHandler : IRequestHandler<DeleteBrandCommand>
    {
        /// <summary>
        /// Database context
        /// </summary>
        private readonly IApplicationDbContext _context;

        /// <summary>
        /// Initializes DeleteBrandHandler
        /// </summary>
        /// <param name="context">Database context</param>
        public DeleteBrandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Handles deleting brand
        /// </summary>
        /// <param name="request">Delete brand request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <exception cref="NotFoundException">Thrown when brand is not found</exception>
        public async Task<Unit> Handle(DeleteBrandCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Brands.FindAsync(request.BrandId);

            if (entity == null) throw new NotFoundException(nameof(Brand), request.BrandId);

            _context.Brands.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}