﻿using System;
using MediatR;

namespace Application.Users.Commands.DeleteUser;

/// <summary>
///     Delete user command
/// </summary>
public class DeleteUserCommand : IRequest
{
    /// <summary>
    ///     User's ID
    /// </summary>
    public Guid UserId { get; init; }

    /// <summary>
    ///     User's password
    /// </summary>
    public string Password { get; init; }
}