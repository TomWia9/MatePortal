using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Opinions.Queries;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Opinions.Commands.CreateOpinion
{
    /// <summary>
    /// Create yerba mate opinion handler
    /// </summary>
    public class CreateYerbaMateOpinionHandler : IRequestHandler<CreateOpinionCommand, OpinionDto>
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
        /// Current user service
        /// </summary>
        private readonly ICurrentUserService _currentUserService;

        /// <summary>
        /// Initializes CreateYerbaMateOpinionHandler
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="mapper">The mapper</param>
        /// <param name="currentUserService">Current user service</param>
        public CreateYerbaMateOpinionHandler(IApplicationDbContext context, IMapper mapper,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// Handles creating yerba mate opinion
        /// </summary>
        /// <param name="request">The create yerba mate opinion request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Opinion data transfer object</returns>
        /// <exception cref="NotFoundException">Thrown when yerba mate is not found</exception>
        public async Task<OpinionDto> Handle(CreateOpinionCommand request, CancellationToken cancellationToken)
        {
            if (!await _context.YerbaMate.AnyAsync(y => y.Id == request.YerbaMateId, cancellationToken))
            {
                throw new NotFoundException(nameof(YerbaMate), request.YerbaMateId);
            }

            if (await _context.Opinions.AnyAsync(o =>
                o.CreatedBy == _currentUserService.UserId && o.YerbaMateId == request.YerbaMateId, cancellationToken))
            {
                throw new ConflictException(nameof(Favourite));
            }

            var entity = new Opinion()
            {
                Rate = request.Rate,
                Comment = request.Comment,
                YerbaMateId = request.YerbaMateId
            };

            //entity.DomainEvents.Add(new YerbaMateOpinionCreatedEvent(entity));
            _context.Opinions.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<OpinionDto>(entity);
        }
    }
}