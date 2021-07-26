using System.Threading;
using System.Threading.Tasks;
using Application.Categories.Queries;
using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Categories.Commands.CreateCategory
{
    /// <summary>
    /// Create category handler
    /// </summary>
    public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, CategoryDto>
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
        /// Initializes CreateCategoryHandler
        /// </summary>
        /// <param name="context">Database context</param>
        /// <param name="mapper">The mapper</param>
        public CreateCategoryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Handles creating category
        /// </summary>
        /// <param name="request">The create category request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Category data transfer object</returns>
        public async Task<CategoryDto> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = new Category()
            {
                Name = request.Name,
                Description = request.Description
            };

            //entity.DomainEvents.Add(new CategoryCreatedEvent(entity));
            _context.Categories.Add(entity);
            await _context.SaveChangesAsync(cancellationToken);

            var brandDto = _mapper.Map<CategoryDto>(entity);

            return brandDto;
        }
    }
}