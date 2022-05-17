using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.ShopOpinions.Queries.GetShopOpinion
{
    /// <summary>
    /// Get single shop opinion handler
    /// </summary>
    public class GetShopOpinionHandler : IRequestHandler<GetShopOpinionQuery, ShopOpinionDto>
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
        /// Initializes GetShopOpinionHandler
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="mapper">The mapper</param>
        public GetShopOpinionHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles getting shop opinion
        /// </summary>
        /// <param name="request">Get shop opinion request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Shop opinion data transfer object</returns>
        /// <exception cref="NotFoundException">Throws when shop opinion is not found</exception>
        public async Task<ShopOpinionDto> Handle(GetShopOpinionQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.ShopOpinions
                .FirstOrDefaultAsync(o => o.Id == request.ShopOpinionId, cancellationToken);

            if (entity == null) throw new NotFoundException(nameof(ShopOpinion), request.ShopOpinionId);

            return _mapper.Map<ShopOpinionDto>(entity);
        }
    }
}