﻿using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.YerbaMateOpinions.Queries;
using AutoMapper;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.YerbaMateOpinions.Commands.CreateYerbaMateOpinion;

/// <summary>
///     Create yerba mate opinion handler
/// </summary>
public class CreateYerbaMateOpinionHandler : IRequestHandler<CreateYerbaMateOpinionCommand, YerbaMateOpinionDto>
{
    /// <summary>
    ///     Database context
    /// </summary>
    private readonly IApplicationDbContext _context;

    /// <summary>
    ///     Current user service
    /// </summary>
    private readonly ICurrentUserService _currentUserService;

    /// <summary>
    ///     The mapper
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    ///     Initializes CreateYerbaMateOpinionHandler
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
    ///     Handles creating yerba mate opinion
    /// </summary>
    /// <param name="request">The create yerba mate opinion request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>Opinion data transfer object</returns>
    /// <exception cref="NotFoundException">Thrown when yerba mate is not found</exception>
    /// <exception cref="ConflictException">Thrown when yerba mate opinion conflicts with another opinion</exception>
    public async Task<YerbaMateOpinionDto> Handle(CreateYerbaMateOpinionCommand request, CancellationToken cancellationToken)
    {
        if (!await _context.YerbaMate.AnyAsync(y => y.Id == request.YerbaMateId, cancellationToken))
            throw new NotFoundException(nameof(YerbaMate), request.YerbaMateId);

        if (await _context.YerbaMateOpinions.AnyAsync(o =>
                o.CreatedBy == _currentUserService.UserId && o.YerbaMateId == request.YerbaMateId, cancellationToken))
            throw new ConflictException(nameof(YerbaMateOpinion));

        var entity = new YerbaMateOpinion
        {
            Rate = request.Rate,
            Comment = request.Comment,
            YerbaMateId = request.YerbaMateId
        };

        //entity.DomainEvents.Add(new YerbaMateOpinionCreatedEvent(entity));
        _context.YerbaMateOpinions.Add(entity);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<YerbaMateOpinionDto>(entity);
    }
}