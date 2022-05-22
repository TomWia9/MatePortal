using System;
using Application.Common.Mappings;
using Domain.Entities;

namespace Application.Countries.Queries;

/// <summary>
///     Country data transfer object
/// </summary>
public class CountryDto : IMapFrom<Country>
{
    /// <summary>
    ///     Country ID
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    ///     Country name
    /// </summary>
    public string Name { get; set; }
}