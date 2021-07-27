using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Favourites.Queries;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Favourites.Commands.CreateFavourite
{
    /// <summary>
    /// Create favourite handler
    /// </summary>
    public class CreateFavouriteHandler : IRequestHandler<CreateFavouriteCommand, FavouriteDto>
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
        /// Initializes CreateFavouriteHandler
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="mapper">The mapper</param>
        public CreateFavouriteHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles creating favourite
        /// </summary>
        /// <param name="request">The create favourite request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Favourite data transfer object</returns>
        /// <exception cref="NotFoundException">Thrown when yerba mate is not found</exception>
        public async Task<FavouriteDto> Handle(CreateFavouriteCommand request, CancellationToken cancellationToken)
        {
            if (!await _context.YerbaMate.AnyAsync(y => y.Id == request.YerbaMateId, cancellationToken))
            {
                throw new NotFoundException(nameof(YerbaMate), request.YerbaMateId);
            }
            
            if (await _context.Favourites.AnyAsync(f =>
                f.CreatedBy == request.UserId && f.YerbaMateId == request.YerbaMateId, cancellationToken))
            {
                throw new ConflictException(nameof(Favourite));
            }

            var entity = new Favourite()
            {
                YerbaMateId = request.YerbaMateId
            };

            //entity.DomainEvents.Add(new FavouriteCreatedEvent(entity));
            _context.Favourites.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            
            //TODO [PATCH] yerba mate numOfAddToFavs (Add YerbaMateService firstly)

            return _mapper.Map<FavouriteDto>(entity);
        }
    }
}