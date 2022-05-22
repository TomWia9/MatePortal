using System;
using System.Linq;

namespace Application.Common.Extensions;

/// <summary>
///     String extensions
/// </summary>
public static class StringExtensions
{
    /// <summary>
    ///     Transforms the first letter of text to uppercase
    /// </summary>
    /// <param name="value">The text value to transform the first letter of</param>
    /// <returns>Transformed string value</returns>
    /// <exception cref="ArgumentNullException">Thrown when value is null</exception>
    /// <exception cref="ArgumentException">Thrown when value is empty</exception>
    public static string FirstCharToUpper(this string value)
    {
        return value switch
        {
            null => throw new ArgumentNullException(nameof(value)),
            "" => throw new ArgumentException($"{nameof(value)} cannot be empty", nameof(value)),
            _ => value.First().ToString().ToUpper() + value[1..]
        };
    }
}