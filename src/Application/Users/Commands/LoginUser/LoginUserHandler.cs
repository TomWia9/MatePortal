using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using MediatR;

namespace Application.Users.Commands.LoginUser
{
    /// <summary>
    ///     Login user handler
    /// </summary>
    public class LoginUserHandler : IRequestHandler<LoginUserCommand, AuthenticationResult>
    {
        /// <summary>
        ///     Identity service
        /// </summary>
        private readonly IIdentityService _identityService;

        /// <summary>
        ///     Initializes LoginUserHandler
        /// </summary>
        /// <param name="identityService">Identity service</param>
        public LoginUserHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        /// <summary>
        ///     Handles login user
        /// </summary>
        /// <param name="request">The login user request</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>AuthenticationResult</returns>
        public async Task<AuthenticationResult> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            return await _identityService.LoginAsync(request.Email, request.Password);
        }
    }
}