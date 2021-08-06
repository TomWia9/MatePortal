using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.YerbaMates.Queries.GetYerbaMate
{
    /// <summary>
    /// Get yerba mate handler
    /// </summary>
    public class GetYerbaMateHandler : IRequestHandler<GetYerbaMateQuery, YerbaMateDto>
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
        /// Initializes GetYerbaMateHandler
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="mapper">The mapper</param>
        public GetYerbaMateHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles getting yerba mate
        /// </summary>
        /// <param name="request">Get yerba mate request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>YerbaMate data transfer object</returns>
        /// <exception cref="NotFoundException">Throws when yerba mate is not found</exception>
        public async Task<YerbaMateDto> Handle(GetYerbaMateQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.YerbaMate
                .Include(y => y.Brand)
                .Include(y => y.Brand.Country)
                .Include(y => y.Category)
                .Include(y => y.Opinions)
                .Include(y => y.Favourites)
                .FirstOrDefaultAsync(y => y.Id == request.YerbaMateId, cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(YerbaMate), request.YerbaMateId);
            }
            
            return _mapper.Map<YerbaMateDto>(entity);
        }
    }
}