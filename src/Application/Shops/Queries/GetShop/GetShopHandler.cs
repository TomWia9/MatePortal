using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Shops.Queries.GetShop
{
    /// <summary>
    /// Get shop handler
    /// </summary>
    public class GetShopHandler : IRequestHandler<GetShopQuery, ShopDto>
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
        /// Initializes GetShopHandler
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="mapper">The mapper</param>
        public GetShopHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles getting shop
        /// </summary>
        /// <param name="request">Get shop request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Shop data transfer object</returns>
        /// <exception cref="NotFoundException">Throws when shop is not found</exception>
        public async Task<ShopDto> Handle(GetShopQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Shops.FirstOrDefaultAsync(s => s.Id == request.ShopId, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Shop), request.ShopId);
            }
            
            return _mapper.Map<ShopDto>(entity);
        }
    }
}