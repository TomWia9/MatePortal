using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Opinions.Commands.CreateOpinion;
using Application.Opinions.Queries;
using Application.ShopOpinions.Queries;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.ShopOpinions.Commands.CreateShopOpinion
{
    /// <summary>
    /// Create shop opinion handler
    /// </summary>
    public class CreateShopOpinionHandler : IRequestHandler<CreateShopOpinionCommand, ShopOpinionDto>
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
        /// Initializes CreateShopOpinionHandler
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="mapper">The mapper</param>
        /// <param name="currentUserService">Current user service</param>
        public CreateShopOpinionHandler(IApplicationDbContext context, IMapper mapper,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        /// <summary>
        /// Handles creating shop opinion
        /// </summary>
        /// <param name="request">The create shop opinion request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Shop opinion data transfer object</returns>
        /// <exception cref="NotFoundException">Thrown when shop is not found</exception>
        /// <exception cref="ConflictException">Thrown when shop opinion conflicts with another shop opinion</exception>
        public async Task<ShopOpinionDto> Handle(CreateShopOpinionCommand request, CancellationToken cancellationToken)
        {
            if (!await _context.Shops.AnyAsync(s => s.Id == request.ShopId, cancellationToken))
            {
                throw new NotFoundException(nameof(Shop), request.ShopId);
            }

            if (await _context.ShopOpinions.AnyAsync(o =>
                o.CreatedBy == _currentUserService.UserId && o.ShopId == request.ShopId, cancellationToken))
            {
                throw new ConflictException(nameof(Favourite));
            }

            var entity = new ShopOpinion()
            {
                Rate = request.Rate,
                Comment = request.Comment,
                ShopId = request.ShopId
            };

            //entity.DomainEvents.Add(new ShopOpinionCreatedEvent(entity));
            _context.ShopOpinions.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return _mapper.Map<ShopOpinionDto>(entity);
        }
    }
}