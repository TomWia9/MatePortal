using System;
using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities;

/// <summary>
///     Shop opinion
/// </summary>
public class ShopOpinion : BaseEntity, IHasDomainEvent
{
    /// <summary>
    ///     The shop opinion ID
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    ///     The shop opinion rate
    /// </summary>
    public int Rate { get; set; }

    /// <summary>
    ///     The shop opinion comment
    /// </summary>
    public string Comment { get; set; }

    /// <summary>
    ///     The shop ID
    /// </summary>
    public Guid ShopId { get; set; }

    /// <summary>
    ///     The Shop
    /// </summary>
    public Shop Shop { get; set; }

    /// <summary>
    ///     Domain events
    /// </summary>
    public List<DomainEvent> DomainEvents { get; set; } = new();
}