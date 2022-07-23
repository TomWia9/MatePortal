using System;
using Application.Common.Interfaces;

namespace Infrastructure.Services;

/// <summary>
///     Date time service
/// </summary>
public class DateTimeService : IDateTime
{
    /// <summary>
    ///     Current Date time
    /// </summary>
    public DateTime Now => DateTime.Now;
}