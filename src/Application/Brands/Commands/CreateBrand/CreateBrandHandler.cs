using System.Threading;
using System.Threading.Tasks;
using Application.Brands.Queries;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Brands.Commands.CreateBrand
{
    /// <summary>
    /// Create brand handler
    /// </summary>
    public class CreateBrandHandler : IRequestHandler<CreateBrandCommand, BrandDto>
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
        /// Initializes CreateBrandHandler
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="mapper">The mapper</param>
        public CreateBrandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles creating brand
        /// </summary>
        /// <param name="request">The create brand request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Brand data transfer object</returns>
        /// <exception cref="NotFoundException">Thrown when country is not found</exception>
        public async Task<BrandDto> Handle(CreateBrandCommand request, CancellationToken cancellationToken)
        {
            if (await _context.Brands.AnyAsync(b => b.Name == request.Name, cancellationToken: cancellationToken))
            {
                throw new ConflictException();
            }
            
            var brandCountry = await _context.Countries.FindAsync(request.CountryId);

            if (brandCountry == null)
            {
                throw new NotFoundException(nameof(Country), request.CountryId);
            }

            var entity = new Brand()
            {
                Name = request.Name,
                CountryId = request.CountryId,
                Description = request.Description
            };

            //entity.DomainEvents.Add(new BrandCreatedEvent(entity));
            _context.Brands.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            var brandDto = _mapper.Map<BrandDto>(entity);
            brandDto.Country = brandCountry.Name;

            return brandDto;
        }
    }
}