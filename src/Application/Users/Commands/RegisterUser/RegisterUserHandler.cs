using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Users.Commands.RegisterUser;

/// <summary>
///     Register user handler
/// </summary>
public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, AuthenticationResult>
{
    /// <summary>
    ///     Identity service
    /// </summary>
    private readonly IIdentityService _identityService;

    /// <summary>
    ///     Initializes RegisterUserHandler
    /// </summary>
    /// <param name="identityService">Identity service</param>
    public RegisterUserHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    /// <summary>
    ///     Handles register user
    /// </summary>
    /// <param name="request">The register user request</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>AuthenticationResult</returns>
    public async Task<AuthenticationResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        return await _identityService.RegisterAsync(request.Email, request.Username, request.Password);
    }
}