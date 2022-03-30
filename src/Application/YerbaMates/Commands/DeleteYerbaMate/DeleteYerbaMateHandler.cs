using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.YerbaMates.Commands.DeleteYerbaMate
{
    /// <summary>
    /// Delete yerba mate handler
    /// </summary>
    public class DeleteYerbaMateHandler : IRequestHandler<DeleteYerbaMateCommand>
    {
        /// <summary>
        /// Database context
        /// </summary>
        private readonly IApplicationDbContext _context;

        /// <summary>
        /// Initializes DeleteYerbaMateHandler
        /// </summary>
        /// <param name="context">Database context</param>
        public DeleteYerbaMateHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Handles deleting yerba mate
        /// </summary>
        /// <param name="request">Delete yerba mate request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <exception cref="NotFoundException">Thrown when yerba mate is not found</exception>
        public async Task<Unit> Handle(DeleteYerbaMateCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.YerbaMate.FindAsync(request.YerbaMateId);

            if (entity == null) throw new NotFoundException(nameof(YerbaMate), request.YerbaMateId);

            _context.YerbaMate.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}