using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Infrastructure.Persistence;

/// <summary>
///     Application database context seed
/// </summary>
public static class ApplicationDbContextSeed
{
    /// <summary>
    ///     Seeds database
    /// </summary>
    /// <param name="userManager">The user manager</param>
    /// <param name="roleManager">The role manager</param>
    /// <param name="context">The Db context</param>
    public static async Task SeedDatabase(UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole<Guid>> roleManager, IApplicationDbContext context)
    {
        await SeedRolesAsync(roleManager);
        await SeedDefaultUserAsync(userManager);
        await SeedCountries(context);
    }

    private static async Task SeedRolesAsync(
        RoleManager<IdentityRole<Guid>> roleManager)
    {
        if (!await roleManager.RoleExistsAsync(Roles.User) ||
            !await roleManager.RoleExistsAsync(Roles.Administrator))
        {
            Log.Logger.Information("Seeding roles");

            if (!await roleManager.RoleExistsAsync(Roles.User))
                await roleManager.CreateAsync(new IdentityRole<Guid>
                {
                    Name = Roles.User,
                    NormalizedName = Roles.User.ToUpper()
                });

            if (!await roleManager.RoleExistsAsync(Roles.Administrator))
                await roleManager.CreateAsync(new IdentityRole<Guid>
                {
                    Name = Roles.Administrator,
                    NormalizedName = Roles.Administrator.ToUpper()
                });
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
                UserName = "Administrator"
            };

            await userManager.CreateAsync(administrator, "Admin123_");
            await userManager.AddToRoleAsync(administrator, "Administrator");
        }
    }

    private static async Task SeedCountries(IApplicationDbContext context)
    {
        if (!await context.Countries.AnyAsync())
        {
            Log.Logger.Information("Seeding countries");

            await context.Countries.AddRangeAsync(GetCountries());
            await context.SaveChangesAsync(CancellationToken.None);
        }
    }

    private static IEnumerable<Country> GetCountries()
    {
        return new List<Country>
        {
            new()
            {
                Id = Guid.Parse("A42066F2-2998-47DC-A193-FF4C4080056F"),
                Name = "Paraguay"
            },
            new()
            {
                Id = Guid.Parse("68E2E690-B2F4-44AE-A21F-756922E25163"),
                Name = "Argentina"
            },
            new()
            {
                Id = Guid.Parse("C08D5B41-C678-421B-9500-93D22004F9CF"),
                Name = "Uruguay"
            },
            new()
            {
                Id = Guid.Parse("A7BBB4DA-12D5-4227-B6BA-690ECF40CD86"),
                Name = "Brazil"
            }
        };
    }
}