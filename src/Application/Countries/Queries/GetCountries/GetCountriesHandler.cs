using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.Common.Mappings;
using AutoMapper;
using MediatR;

namespace Application.Countries.Queries.GetCountries
{
    /// <summary>
    /// Get countries handler
    /// </summary>
    public class GetCountriesHandler : IRequestHandler<GetCountriesQuery, IEnumerable<CountryDto>>
    {
        /// <summary>
        /// Database context
        /// </summary>
        private readonly IApplicationDbContext _context;

        /// <summary>
        /// Initializes GetCountriesHandler
        /// </summary>
        /// <param name="context">Database context</param>
        public GetCountriesHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Handles getting countries
        /// </summary>
        /// <param name="request">Get countries request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of country data transfer objects</returns>
        public async Task<IEnumerable<CountryDto>> Handle(GetCountriesQuery request,
            CancellationToken cancellationToken)
        {
            var countries = _context.Countries.AsQueryable();

            var mapperConfiguration = new MapperConfiguration(cfg => { cfg.AddProfile<MappingProfile>(); });

            return await countries.ProjectToListAsync<CountryDto>(mapperConfiguration);
        }
    }
}