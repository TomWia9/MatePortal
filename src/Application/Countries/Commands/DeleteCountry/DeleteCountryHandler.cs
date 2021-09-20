using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Countries.Commands.DeleteCountry
{
    /// <summary>
    ///     Delete country handler
    /// </summary>
    public class DeleteCountryHandler : IRequestHandler<DeleteCountryCommand>
    {
        /// <summary>
        ///     Database context
        /// </summary>
        private readonly IApplicationDbContext _context;

        /// <summary>
        ///     Initializes DeleteCountryHandler
        /// </summary>
        /// <param name="context">Database context</param>
        public DeleteCountryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        ///     Handles deleting country
        /// </summary>
        /// <param name="request">Delete country request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <exception cref="NotFoundException">Thrown when country is not found</exception>
        public async Task<Unit> Handle(DeleteCountryCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Countries.FindAsync(request.CountryId);

            if (entity == null) throw new NotFoundException(nameof(Country), request.CountryId);

            _context.Countries.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}