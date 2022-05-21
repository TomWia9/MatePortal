using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.ShopOpinions.Commands.DeleteShopOpinion
{
    /// <summary>
    ///     Delete shop opinion handler
    /// </summary>
    public class DeleteShopOpinionHandler : IRequestHandler<DeleteShopOpinionCommand>
    {
        /// <summary>
        ///     Database context
        /// </summary>
        private readonly IApplicationDbContext _context;

        /// <summary>
        ///     Current user service
        /// </summary>
        private readonly ICurrentUserService _currentUserService;

        /// <summary>
        ///     Initializes DeleteShopOpinionHandler
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="currentUserService">Current user service</param>
        public DeleteShopOpinionHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        /// <summary>
        ///     Handles deleting shop opinion
        /// </summary>
        /// <param name="request">Delete shop opinion request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <exception cref="NotFoundException">Thrown when shop opinion is not found</exception>
        public async Task<Unit> Handle(DeleteShopOpinionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.ShopOpinions.FindAsync(request.ShopOpinionId);

            if (entity == null) throw new NotFoundException(nameof(ShopOpinion), request.ShopOpinionId);

            if (!_currentUserService.AdministratorAccess &&
                entity.CreatedBy != _currentUserService.UserId)
                throw new ForbiddenAccessException();

            _context.ShopOpinions.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}