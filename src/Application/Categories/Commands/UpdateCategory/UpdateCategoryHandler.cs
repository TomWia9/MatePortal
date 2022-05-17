using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Categories.Commands.UpdateCategory
{
    /// <summary>
    /// Update category handler
    /// </summary>
    public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand>
    {
        /// <summary>
        /// Database context
        /// </summary>
        private readonly IApplicationDbContext _context;

        /// <summary>
        /// Initializes UpdateCategoryHandler
        /// </summary>
        /// <param name="context">Database context</param>
        public UpdateCategoryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Handles updating category
        /// </summary>
        /// <param name="request">Update category request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <exception cref="NotFoundException">Thrown when category is not found</exception>
        /// <exception cref="ConflictException">Thrown when category conflicts with another category</exception>
        public async Task<Unit> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Categories.FindAsync(request.CategoryId);

            if (entity == null) throw new NotFoundException(nameof(Category), request.CategoryId);

            if (await _context.Categories.AnyAsync(c => c.Name == request.Name && entity.Name == request.Name,
                cancellationToken))
                throw new ConflictException();

            entity.Name = request.Name;
            entity.Description = request.Description;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}