using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Countries.Commands.UpdateCountry
{
    /// <summary>
    /// Update country handler
    /// </summary>
    public class UpdateCountryHandler : IRequestHandler<UpdateCountryCommand>
    {
        /// <summary>
        /// Database context
        /// </summary>
        private readonly IApplicationDbContext _context;

        /// <summary>
        /// Initializes UpdateCountryHandler
        /// </summary>
        /// <param name="context">Database context</param>
        public UpdateCountryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Handles updating country 
        /// </summary>
        /// <param name="request">Update country request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <exception cref="NotFoundException">Thrown when country not found</exception>
        public async Task<Unit> Handle(UpdateCountryCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Countries.FindAsync(request.CountryId);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Country), request.CountryId);
            }

            entity.Name = request.Name;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}