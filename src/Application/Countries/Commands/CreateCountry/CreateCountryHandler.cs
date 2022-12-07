using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Countries.Queries;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Countries.Commands.CreateCountry;

/// <summary>
///     Create country handler
/// </summary>
public class CreateCountryHandler : IRequestHandler<CreateCountryCommand, CountryDto>
{
    /// <summary>
    ///     Database context
    /// </summary>
    private readonly IApplicationDbContext _context;

    /// <summary>
    ///     The mapper
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    ///     Initializes CreateCountryHandler
    /// </summary>
    /// <param name="context">Database context</param>
    /// <param name="mapper">The mapper</param>
    public CreateCountryHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    ///     Handles creating country
    /// </summary>
    /// <param name="request">Create country request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Country data transfer object</returns>
    /// <exception cref="ConflictException">Thrown when country conflicts with another country</exception>
    public async Task<CountryDto> Handle(CreateCountryCommand request, CancellationToken cancellationToken)
    {
        if (await _context.Countries.AnyAsync(c => c.Name == request.Name,
                cancellationToken))
            throw new ConflictException(nameof(Country));

        var entity = new Country
        {
            Name = request.Name,
            Description = request.Description
        };

        //entity.DomainEvents.Add(new CountryCreatedEvent(entity));
        _context.Countries.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        var countryDto = _mapper.Map<CountryDto>(entity);

        return countryDto;
    }
}