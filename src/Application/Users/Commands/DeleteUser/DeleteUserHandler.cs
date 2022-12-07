using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Users.Commands.DeleteUser;

/// <summary>
///     Delete user handler
/// </summary>
public class DeleteUserHandler : IRequestHandler<DeleteUserCommand>
{
    /// <summary>
    ///     Current user service
    /// </summary>
    private readonly ICurrentUserService _currentUserService;

    /// <summary>
    ///     User service
    /// </summary>
    private readonly IUsersService _userService;

    /// <summary>
    ///     Initializes DeleteUserHandler
    /// </summary>
    /// <param name="userService">User service</param>
    /// <param name="currentUserService">Current user service</param>
    public DeleteUserHandler(IUsersService userService, ICurrentUserService currentUserService)
    {
        _userService = userService;
        _currentUserService = currentUserService;
    }

    /// <summary>
    ///     Handles deleting user
    /// </summary>
    /// <param name="request">The delete user request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var currentUserId = _currentUserService.UserId;
        
        if (request.UserId != currentUserId && !_currentUserService.AdministratorAccess)
            throw new ForbiddenAccessException();
        
        await _userService.DeleteUserAsync(request, _currentUserService.AdministratorAccess);

        return Unit.Value;
    }
}