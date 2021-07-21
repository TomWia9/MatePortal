using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Brands.Commands.UpdateBrand
{
    /// <summary>
    /// Update brand handler
    /// </summary>
    public class UpdateBrandHandler : IRequestHandler<UpdateBrandCommand>
    {
        /// <summary>
        /// Database context
        /// </summary>
        private readonly IApplicationDbContext _context;

        /// <summary>
        /// Initializes UpdateBrandHandler
        /// </summary>
        /// <param name="context">Database context</param>
        public UpdateBrandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Handles updating brand 
        /// </summary>
        /// <param name="request">Update brand request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <exception cref="NotFoundException">Thrown when brand is not found</exception>
        public async Task<Unit> Handle(UpdateBrandCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Brands.FindAsync(request.BrandId);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Brand), request.BrandId);
            }

            if (!await _context.Countries.AnyAsync(c => c.Id == request.CountryId,
                cancellationToken))
            {
                throw new NotFoundException(nameof(Country), request.CountryId);
            }

            entity.Name = request.Name;
            entity.Description = request.Description;
            entity.CountryId = request.CountryId;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}