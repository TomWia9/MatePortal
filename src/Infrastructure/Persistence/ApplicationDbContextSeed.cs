using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Infrastructure.Persistence
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedDatabase(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole<Guid>> roleManager)
        {
            await SeedRolesAsync(roleManager);
            await SeedDefaultUserAsync(userManager);
        }

        private static async Task SeedRolesAsync(
            RoleManager<IdentityRole<Guid>> roleManager)
        {
            //TODO change hardcoded roles to roles from Roles class
            if (!await roleManager.RoleExistsAsync("User") || !await roleManager.RoleExistsAsync("Administrator"))
            {
                Log.Logger.Information("Seeding roles");

                if (!await roleManager.RoleExistsAsync("User"))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>()
                    {
                        Name = "User",
                        NormalizedName = "USER"
                    });
                }

                if (!await roleManager.RoleExistsAsync("Administrator"))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>()
                    {
                        Name = "Administrator",
                        NormalizedName = "ADMINISTRATOR"
                    });
                }
            }
        }

        private static async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager)
        {
            var usersInAdministratorRole = await userManager.GetUsersInRoleAsync("Administrator");

            if (!usersInAdministratorRole.Any())
            {
                Log.Logger.Information("Seeding default user");
                var administrator = new ApplicationUser
                {
                    Email = "admin@admin.com",
                    UserName = "Administrator",
                };

                await userManager.CreateAsync(administrator, "Admin123_");
                await userManager.AddToRoleAsync(administrator, "Administrator");
            }
        }
    }
}