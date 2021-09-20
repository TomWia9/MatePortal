using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Brands.Queries.GetBrand
{
    /// <summary>
    ///     Get brand handler
    /// </summary>
    public class GetBrandHandler : IRequestHandler<GetBrandQuery, BrandDto>
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
        ///     Initializes GetBrandHandler
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="mapper">The mapper</param>
        public GetBrandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        ///     Handles getting brand
        /// </summary>
        /// <param name="request">Get brand request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Brand data transfer object</returns>
        /// <exception cref="NotFoundException">Throws when brand is not found</exception>
        public async Task<BrandDto> Handle(GetBrandQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Brands.Include(b => b.Country)
                .FirstOrDefaultAsync(b => b.Id == request.BrandId, cancellationToken);

            if (entity == null) throw new NotFoundException(nameof(Brand), request.BrandId);

            return _mapper.Map<BrandDto>(entity);
        }
    }
}