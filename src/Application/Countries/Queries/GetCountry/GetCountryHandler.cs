using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Countries.Queries.GetCountry
{
    /// <summary>
    /// Get country handler
    /// </summary>
    public class GetCountryHandler : IRequestHandler<GetCountryQuery, CountryDto>
    {
        /// <summary>
        /// Database context
        /// </summary>
        private readonly IApplicationDbContext _context;

        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes GetCountryHandler
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="mapper">The mapper</param>
        public GetCountryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles getting country
        /// </summary>
        /// <param name="request">Get country request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Country data transfer object</returns>
        /// <exception cref="NotFoundException">Throws when country is not found</exception>
        public async Task<CountryDto> Handle(GetCountryQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Countries.FindAsync(request.CountryId);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Country), request.CountryId);
            }

            return _mapper.Map<CountryDto>(entity);
        }
    }
}