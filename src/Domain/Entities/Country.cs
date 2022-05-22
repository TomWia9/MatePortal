using System;
using System.Collections.Generic;

namespace Domain.Entities;

/// <summary>
///     The country
/// </summary>
public class Country
{
    /// <summary>
    ///     The country ID
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    ///     The country name
    /// </summary>
    public string Name { get; set; }
    // TODO Add description to Country

    /// <summary>
    ///     The country brands
    /// </summary>
    public IList<Brand> Brands { get; set; } = new List<Brand>();
}