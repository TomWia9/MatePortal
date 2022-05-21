using System.Collections.Generic;

namespace Application.Users;

/// <summary>
///     Authentication result
/// </summary>
public class AuthenticationResult
{
    /// <summary>
    ///     Authentication token
    /// </summary>
    public string Token { get; init; }

    /// <summary>
    ///     Indicates success
    /// </summary>
    public bool Success { get; init; }

    /// <summary>
    ///     Error messages list
    /// </summary>
    public IEnumerable<string> ErrorMessages { get; init; }
}