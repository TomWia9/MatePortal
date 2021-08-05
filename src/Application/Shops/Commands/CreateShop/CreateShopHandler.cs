using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Shops.Queries;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Shops.Commands.CreateShop
{
    /// <summary>
    /// Create shop handler
    /// </summary>
    public class CreateShopHandler : IRequestHandler<CreateShopCommand, ShopDto>
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
        /// Initializes CreateShopHandler
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="mapper">The mapper</param>
        public CreateShopHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles creating shop
        /// </summary>
        /// <param name="request">The create shop request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Shop data transfer object</returns>
        /// <exception cref="ConflictException">Thrown when shop name conflicts with another shop name</exception>
        public async Task<ShopDto> Handle(CreateShopCommand request, CancellationToken cancellationToken)
        {
            if (await _context.Shops.AnyAsync(s => s.Name == request.Name, cancellationToken: cancellationToken))
            {
                throw new ConflictException();
            }

            var entity = new Shop()
            {
                Name = request.Name,
                Description = request.Description
            };

            //entity.DomainEvents.Add(new ShopCreatedEvent(entity));
            _context.Shops.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            var shopDto = _mapper.Map<ShopDto>(entity);

            return shopDto;
        }
    }
}