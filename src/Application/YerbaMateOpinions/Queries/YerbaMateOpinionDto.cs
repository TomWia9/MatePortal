using System;
using Application.Common.Mappings;
using Domain.Entities;

namespace Application.YerbaMateOpinions.Queries;

/// <summary>
///     Yerba mate opinion data transfer object
/// </summary>
public class YerbaMateOpinionDto : IMapFrom<YerbaMateOpinion>
{
    /// <summary>
    ///     Yerba mate opinion ID
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    ///     Yerba mate opinion rate
    /// </summary>
    public int Rate { get; init; }

    /// <summary>
    ///     Yerba mate opinion comment
    /// </summary>
    public string Comment { get; init; }

    /// <summary>
    ///     Yerba mate opinion created date
    /// </summary>
    public DateTime Created { get; set; }

    /// <summary>
    ///     Yerba mate ID
    /// </summary>
    public Guid YerbaMateId { get; set; }

    /// <summary>
    ///     User ID
    /// </summary>
    public Guid CreatedBy { get; set; }
}