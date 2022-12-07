using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Countries.Commands.UpdateCountry;

/// <summary>
///     Update country handler
/// </summary>
public class UpdateCountryHandler : IRequestHandler<UpdateCountryCommand>
{
    /// <summary>
    ///     Database context
    /// </summary>
    private readonly IApplicationDbContext _context;

    /// <summary>
    ///     Initializes UpdateCountryHandler
    /// </summary>
    /// <param name="context">Database context</param>
    public UpdateCountryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    ///     Handles updating country
    /// </summary>
    /// <param name="request">Update country request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <exception cref="NotFoundException">Thrown when country not found</exception>
    /// <exception cref="ConflictException">Thrown when country conflicts with another country</exception>
    public async Task<Unit> Handle(UpdateCountryCommand request, CancellationToken cancellationToken)
    {
        var entity = await _context.Countries.FindAsync(request.CountryId);

        if (entity == null) throw new NotFoundException(nameof(Country), request.CountryId);

        if (await _context.Countries.Where(x => x != entity).AnyAsync(c => c.Name == request.Name,
                cancellationToken))
            throw new ConflictException(nameof(Country));

        entity.Name = request.Name;
        entity.Description = request.Description;

        await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}