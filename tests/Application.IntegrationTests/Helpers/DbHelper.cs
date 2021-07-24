using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.IntegrationTests.Helpers
{
    public static class DbHelper
    {
        public static async Task SeedTestBrands(IApplicationDbContext context)
        {
            if (!await context.Brands.AnyAsync())
            {
                await context.Brands.AddRangeAsync(GetBrands());
                await context.SaveChangesAsync(CancellationToken.None);
            }
        }

        private static IEnumerable<Brand> GetBrands()
        {
            return new List<Brand>
            {
                new()
                {
                    Id = Guid.NewGuid(),
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
    }
}