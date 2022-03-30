using System;

namespace Application.Common.Interfaces
{
    /// <summary>
    /// DateTime interface
    /// </summary>
    public interface IDateTime
    {
        /// <summary>
        /// Current date time
        /// </summary>
        DateTime Now { get; }
    }
}