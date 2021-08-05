using System.Threading;
using System.Threading.Tasks;
using Application.Categories.Queries;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.YerbaMates.Queries;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.YerbaMates.Commands.CreateYerbaMate
{
    /// <summary>
    /// Create yerba mate handler
    /// </summary>
    public class CreateYerbaMateHandler : IRequestHandler<CreateYerbaMateCommand, YerbaMateDto>
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
        /// Initializes CreateYerbaMateHandler
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="mapper">The mapper</param>
        public CreateYerbaMateHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles creating yerba mate
        /// </summary>
        /// <param name="request">The create yerba mate request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>YerbaMate data transfer object</returns>
        /// <exception cref="NotFoundException">Thrown when brand or category is not found</exception>
        /// <exception cref="ConflictException">Thrown when yerba mate name conflicts with another yerba mate name</exception>
        public async Task<YerbaMateDto> Handle(CreateYerbaMateCommand request, CancellationToken cancellationToken)
        {
            if (await _context.YerbaMate.AnyAsync(s => s.Name == request.Name, cancellationToken: cancellationToken))
            {
                throw new ConflictException();
            }

            if (!await _context.Brands.AnyAsync(b => b.Id == request.BrandId, cancellationToken: cancellationToken))
            {
                throw new NotFoundException(nameof(Brand), request.BrandId);
            }

            if (!await _context.Categories.AnyAsync(c => c.Id == request.CategoryId,
                cancellationToken: cancellationToken))
            {
                throw new NotFoundException(nameof(Category), request.CategoryId);
            }

            var entity = new YerbaMate()
            {
                Name = request.Name,
                Description = request.Description,
                imgUrl = request.imgUrl,
                AveragePrice = request.AveragePrice,
                CategoryId = request.CategoryId,
                BrandId = request.BrandId
            };

            //entity.DomainEvents.Add(new YerbaMateCreatedEvent(entity));
            _context.YerbaMate.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);
            
            entity = await _context.YerbaMate
                .Include(y => y.Brand)
                .Include(y => y.Brand.Country)
                .Include(y => y.Category).FirstOrDefaultAsync(y => y.Id == entity.Id, cancellationToken: cancellationToken);

            var yerbaMateDto = _mapper.Map<YerbaMateDto>(entity);

            return yerbaMateDto;
        }
    }
}