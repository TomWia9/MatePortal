using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Opinions.Queries.GetYerbaMateOpinion
{
    /// <summary>
    /// Get single yerba mate opinion handler
    /// </summary>
    public class GetYerbaMateOpinionHandler : IRequestHandler<GetYerbaMateOpinionQuery, OpinionDto>
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
        /// Initializes GetYerbaMateOpinionHandler
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="mapper">The mapper</param>
        public GetYerbaMateOpinionHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles getting yerba mate opinion
        /// </summary>
        /// <param name="request">Get yerba mate opinion request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Opinion data transfer object</returns>
        /// <exception cref="NotFoundException">Throws when opinion is not found</exception>
        public async Task<OpinionDto> Handle(GetYerbaMateOpinionQuery request, CancellationToken cancellationToken)
        {
            var entity = await _context.Opinions
                .FirstOrDefaultAsync(o => o.Id == request.OpinionId, cancellationToken);

            if (entity == null) throw new NotFoundException(nameof(Opinion), request.OpinionId);

            return _mapper.Map<OpinionDto>(entity);
        }
    }
}