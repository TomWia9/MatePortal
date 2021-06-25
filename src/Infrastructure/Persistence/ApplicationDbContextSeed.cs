using System;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
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
            if (!await roleManager.RoleExistsAsync(Roles.User) ||
                !await roleManager.RoleExistsAsync(Roles.Administrator))
            {
                Log.Logger.Information("Seeding roles");

                if (!await roleManager.RoleExistsAsync(Roles.User))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>()
                    {
                        Name = Roles.User,
                        NormalizedName = Roles.User.ToUpper()
                    });
                }

                if (!await roleManager.RoleExistsAsync(Roles.Administrator))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>()
                    {
                        Name = Roles.Administrator,
                        NormalizedName = Roles.Administrator.ToUpper()
                    });
                }
            }
        }

        private static async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager)
        {
            var usersInAdministratorRole = await userManager.GetUsersInRoleAsync(Roles.Administrator);

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