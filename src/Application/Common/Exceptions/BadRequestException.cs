using System;

namespace Application.Common.Exceptions;

/// <summary>
///     Bad request exception
/// </summary>
public class BadRequestException : Exception
{
    /// <summary>
    ///     Initializes default BadRequestException
    /// </summary>
    public BadRequestException()
    {
    }

    /// <summary>
    ///     Initializes BadRequestException with message
    /// </summary>
    /// <param name="message">The message</param>
    public BadRequestException(string message) : base(message)
    {
    }
}