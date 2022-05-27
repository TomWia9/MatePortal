using System;
using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities;

/// <summary>
///     The yerba mate opinion
/// </summary>
public class YerbaMateOpinion : BaseEntity, IHasDomainEvent
{
    /// <summary>
    ///     The yerba mate opinion ID
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    ///     The rate
    /// </summary>
    public int Rate { get; set; }

    /// <summary>
    ///     The comment
    /// </summary>
    public string Comment { get; set; }

    /// <summary>
    ///     The yerba mate ID
    /// </summary>
    public Guid YerbaMateId { get; set; }

    /// <summary>
    ///     The yerba mate
    /// </summary>
    public YerbaMate YerbaMate { get; set; }

    /// <summary>
    ///     Domain events
    /// </summary>
    public List<DomainEvent> DomainEvents { get; set; } = new();
}