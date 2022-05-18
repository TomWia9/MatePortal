using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Api.Common;
using Application.Common.Interfaces;
using Application.Users;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Api.Services
{
    /// <summary>
    ///     The identity service
    /// </summary>
    public class IdentityService : IIdentityService
    {
        /// <summary>
        ///     The json web token settings
        /// </summary>
        private readonly JwtSettings _jwtSettings;

        /// <summary>
        ///     The user manager
        /// </summary>
        private readonly UserManager<ApplicationUser> _userManager;

        /// <summary>
        ///     Initializes the IdentityService
        /// </summary>
        /// <param name="userManager">TThe user manager</param>
        /// <param name="jwtSettings">The user manager</param>
        public IdentityService(UserManager<ApplicationUser> userManager, JwtSettings jwtSettings)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
        }

        /// <summary>
        ///     Registers the user
        /// </summary>
        /// <param name="email">The email</param>
        /// <param name="username">The username</param>
        /// <param name="password">The password</param>
        /// <returns>An AuthenticationResult</returns>
        public async Task<AuthenticationResult> RegisterAsync(string email, string username, string password)
        {
            var userExists = await _userManager.FindByEmailAsync(email);
            if (userExists != null)
                return new AuthenticationResult
                {
                    ErrorMessages = new[] {"User with this email already exists"}
                };

            var newUser = new ApplicationUser
            {
                Email = email,
                UserName = username
            };

            var createdUser = await _userManager.CreateAsync(newUser, password);

            if (!createdUser.Succeeded)
                return new AuthenticationResult
                {
                    ErrorMessages = createdUser.Errors.Select(x => x.Description)
                };

            await _userManager.AddToRoleAsync(newUser, Roles.User);

            return await GenerateAuthenticationResult(newUser);
        }

        /// <summary>
        ///     Authenticates the user
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>An AuthenticationResult</returns>
        public async Task<AuthenticationResult> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return new AuthenticationResult
                {
                    ErrorMessages = new[] {"User doesn't exist"}
                };

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);

            if (!isPasswordValid)
                return new AuthenticationResult
                {
                    ErrorMessages = new[] {"Password is wrong"}
                };

            return await GenerateAuthenticationResult(user);
        }

        /// <summary>
        ///     Generates authentication result
        /// </summary>
        /// <param name="user">The user</param>
        /// <returns>An AuthenticationResult</returns>
        private async Task<AuthenticationResult> GenerateAuthenticationResult(ApplicationUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email),
                new("id", user.Id.ToString())
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            claims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthenticationResult
            {
                Success = true,
                Token = tokenHandler.WriteToken(token)
            };
        }
    }
}