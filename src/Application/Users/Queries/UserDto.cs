using System;

namespace Application.Users.Queries;

/// <summary>
///     User data transfer object
/// </summary>
public class UserDto
{
    /// <summary>
    ///     User's ID
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    ///     User's email
    /// </summary>
    public string Email { get; init; }

    /// <summary>
    ///     User's username
    /// </summary>
    public string Username { get; init; }

    /// <summary>
    ///     User's role
    /// </summary>
    public string Role { get; init; }
}