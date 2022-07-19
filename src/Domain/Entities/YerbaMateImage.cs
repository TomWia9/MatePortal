using System;
using System.Collections.Generic;
using Domain.Common;

namespace Domain.Entities;

/// <summary>
///     Yerba mate image
/// </summary>
public class YerbaMateImage : BaseEntity, IHasDomainEvent
{
    /// <summary>
    ///     The yerba mate image ID
    /// </summary>
    public Guid Id { get; init; }
    
    /// <summary>
    ///     The image url
    /// </summary>
    public string Url { get; set; }

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