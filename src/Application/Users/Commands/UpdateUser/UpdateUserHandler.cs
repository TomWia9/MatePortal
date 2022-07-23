using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Users.Commands.UpdateUser;

/// <summary>
///     Update user handler
/// </summary>
public class UpdateUserHandler : IRequestHandler<UpdateUserCommand>
{
    /// <summary>
    ///     User service
    /// </summary>
    private readonly IUsersService _userService;

    /// <summary>
    /// Current user service
    /// </summary>
    private readonly ICurrentUserService _currentUserService;

    /// <summary>
    ///     Initializes UpdateUserHandler
    /// </summary>
    /// <param name="userService">User service</param>
    /// <param name="currentUserService">Current user service</param>
    public UpdateUserHandler(IUsersService userService, ICurrentUserService currentUserService)
    {
        _userService = userService;
        _currentUserService = currentUserService;
    }

    /// <summary>
    ///     Handles updating user
    /// </summary>
    /// <param name="request">The update user request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;

        if (request.UserId != currentUserId && !_currentUserService.AdministratorAccess)
            throw new ForbiddenAccessException();

        await _userService.UpdateUserAsync(request);

        return Unit.Value;
    }
}