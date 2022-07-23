using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.YerbaMateImages.Queries;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.YerbaMateImages.Commands.CreateYerbaMateImage;

/// <summary>
///     Create yerba mate image handler
/// </summary>
public class CreateYerbaMateImageHandler : IRequestHandler<CreateYerbaMateImageCommand, YerbaMateImageDto>
{
    /// <summary>
    ///     Database context
    /// </summary>
    private readonly IApplicationDbContext _context;

    /// <summary>
    ///     The mapper
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    ///     Initializes CreateYerbaMateImageHandler
    /// </summary>
    /// <param name="context">Database context</param>
    /// <param name="mapper">The mapper</param>
    public CreateYerbaMateImageHandler(IApplicationDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    /// <summary>
    ///     Handles creating yerba mate image
    /// </summary>
    /// <param name="request">The create yerba mate image request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Opinion data transfer object</returns>
    /// <exception cref="NotFoundException">Thrown when yerba mate is not found</exception>
    public async Task<YerbaMateImageDto> Handle(CreateYerbaMateImageCommand request,
        CancellationToken cancellationToken)
    {
        if (!await _context.YerbaMate.AnyAsync(y => y.Id == request.YerbaMateId, cancellationToken))
            throw new NotFoundException(nameof(YerbaMate), request.YerbaMateId);
        
        var entity = new YerbaMateImage
        {
            Url = request.Url,
            YerbaMateId = request.YerbaMateId
        };

        //entity.DomainEvents.Add(new YerbaMateImageCreatedEvent(entity));
        _context.YerbaMateImages.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<YerbaMateImageDto>(entity);
    }
}