using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Application.IntegrationTests.Helpers
{
    /// <summary>
    /// Test seeder for seeding test data to database
    /// </summary>
    public static class TestSeeder
    {
        /// <summary>
        /// Seed test brands
        /// </summary>
        /// <param name="context">Database context</param>
        public static async Task SeedTestBrands(IApplicationDbContext context)
        {
            if (!await context.Brands.AnyAsync())
            {
                await context.Brands.AddRangeAsync(GetBrands());
                await context.SaveChangesAsync(CancellationToken.None);
            }
        }

        /// <summary>
        /// Seed test categories
        /// </summary>
        /// <param name="factory">Web application factory</param>
        public static async Task SeedTestCategories(CustomWebApplicationFactory factory)
        {
            var context = GetDbContext(factory);

            if (!await context.Categories.AnyAsync())
            {
                await context.Categories.AddRangeAsync(GetCategories());
                await context.SaveChangesAsync(CancellationToken.None);
            }
        }
        
        /// <summary>
        /// Gets database context
        /// </summary>
        /// <param name="factory">Web application factory</param>
        /// <returns>ApplicationDbContext service</returns>
        private static IApplicationDbContext GetDbContext(CustomWebApplicationFactory factory)
        {
            var scope = factory.Services.CreateScope();
            return scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        }

        /// <summary>
        /// Gets test brands
        /// </summary>
        /// <returns>List of test brands</returns>
        private static IEnumerable<Brand> GetBrands()
        {
            return new List<Brand>
            {
                new()
                {
                    Id = Guid.Parse("17458BDE-3849-4150-B73A-A492A8F7F239"),
                    Name = "Kurupi",
                    Description = "Kurupi description",
                    CountryId = Guid.Parse("A42066F2-2998-47DC-A193-FF4C4080056F"),
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Pajarito",
                    Description = "Pajarito description",
                    CountryId = Guid.Parse("A42066F2-2998-47DC-A193-FF4C4080056F"),
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Cruz de malta",
                    Description = "Cruz de malta description",
                    CountryId = Guid.Parse("68E2E690-B2F4-44AE-A21F-756922E25163"),
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Erva Mate",
                    Description = "Erva Mate description",
                    CountryId = Guid.Parse("A7BBB4DA-12D5-4227-B6BA-690ECF40CD86"),
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Baldo",
                    Description = "Baldo description",
                    CountryId = Guid.Parse("C08D5B41-C678-421B-9500-93D22004F9CF"),
                }
            };
        }

        /// <summary>
        /// Gets test categories
        /// </summary>
        /// <returns>List of test categories</returns>
        private static IEnumerable<Category> GetCategories()
        {
            return new List<Category>
            {
                new()
                {
                    Id = Guid.Parse("8438FB5B-DC77-40F2-ABB6-C7DCE326571E"),
                    Name = "Herbal",
                    Description = "Herbal description",
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Fruit",
                    Description = "Fruit description",
                }
            };
        }
    }
}