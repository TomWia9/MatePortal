using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Application.Users;
using Application.Users.Commands.RegisterUser;
using Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace Application.IntegrationTests.Helpers
{
    /// <summary>
    ///     Auth helper
    /// </summary>
    public static class AuthHelper
    {
        /// <summary>
        ///     Runs as default user
        /// </summary>
        /// <param name="factory">The factory instance</param>
        /// <returns>User ID</returns>
        public static async Task<Guid> RunAsDefaultUserAsync(CustomWebApplicationFactory factory)
        {
            return await RunAsUserAsync(factory, "user@test.com", "User123_", Roles.User);
        }

        /// <summary>
        ///     Runs as administrator
        /// </summary>
        /// <param name="factory">The factory instance</param>
        /// <returns>Administrator ID</returns>
        public static async Task<Guid> RunAsAdministratorAsync(CustomWebApplicationFactory factory)
        {
            return await RunAsUserAsync(factory, "administrator@admin.com", "Admin123_", Roles.Administrator);
        }

        /// <summary>
        ///     Gets user by jwt token
        /// </summary>
        /// <param name="mediator">The mediator instance</param>
        /// <returns></returns>
        public static async Task<AuthenticationResult> RegisterTestUserAsync(ISender mediator)
        {
            var registerUserCommand = new RegisterUserCommand
            {
                Email = "test@test.com",
                Username = "Test",
                Password = "Qwerty123_"
            };

            return await mediator.Send(registerUserCommand);
        }

        public static async Task<ApplicationUser> GetUserByTokenAsync(CustomWebApplicationFactory factory, string jwt)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.ReadJwtToken(jwt);
            var userId = token.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

            return await DbHelper.FindAsync<ApplicationUser>(factory, Guid.Parse(userId));
        }

        /// <summary>
        ///     Runs as user
        /// </summary>
        /// <param name="factory">The factory instance</param>
        /// <param name="userName">User name</param>
        /// <param name="password">User's password</param>
        /// <param name="role">User's role></param>
        /// <returns>User or administrator ID</returns>
        private static async Task<Guid> RunAsUserAsync(CustomWebApplicationFactory factory, string userName,
            string password, string role)
        {
            using var scope = factory.Services.CreateScope();
            var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetService<RoleManager<IdentityRole<Guid>>>();

            if (userManager == null || roleManager == null) throw new Exception("Failed to get user or role manager");

            var user = new ApplicationUser { Email = userName, UserName = userName };

            var createdUser = await userManager.CreateAsync(user, password);

            if (createdUser.Succeeded)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole<Guid>
                    {
                        Name = role,
                        NormalizedName = role.ToUpper()
                    });

                await userManager.AddToRoleAsync(user, role);

                factory.CurrentUserId = user.Id;
                factory.CurrentUserRole = role;

                return factory.CurrentUserId;
            }

            var errors = string.Join(Environment.NewLine, createdUser.Errors);

            throw new Exception($"Unable to create {userName}.{Environment.NewLine}{errors}");
        }
    }
}