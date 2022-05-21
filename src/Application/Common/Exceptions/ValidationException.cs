using System;
using System.Collections.Generic;

namespace Application.Common.Exceptions;

/// <summary>
///     Validation exception
/// </summary>
public class ValidationException : Exception
{
    /// <summary>
    ///     Initializes default ValidationException with message
    /// </summary>
    public ValidationException() : base("One or more validation failures have occured")
    {
        Errors = new Dictionary<string, string[]>();
    }

    /// <summary>
    ///     The errors
    /// </summary>
    public IDictionary<string, string[]> Errors { get; }

    //TODO Add second constructor with failures from validator when fluent validation will be added
}