﻿using System;
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
        /// <param name="factory">Web application factory</param>
        public static async Task SeedTestBrandsAsync(CustomWebApplicationFactory factory)
        {
            var context = GetDbContext(factory);

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
        public static async Task SeedTestCategoriesAsync(CustomWebApplicationFactory factory)
        {
            var context = GetDbContext(factory);

            if (!await context.Categories.AnyAsync())
            {
                await context.Categories.AddRangeAsync(GetCategories());
                await context.SaveChangesAsync(CancellationToken.None);
            }
        }

        /// <summary>
        /// Seed test favourites
        /// </summary>
        /// <param name="factory">Web application factory</param>
        /// <param name="userId">User ID</param>
        public static async Task SeedTestFavouritesAsync(CustomWebApplicationFactory factory)
        {
            var context = GetDbContext(factory);

            if (!await context.Favourites.AnyAsync())
            {
                await context.Favourites.AddRangeAsync(GetFavourites());
                await context.SaveChangesAsync(CancellationToken.None);
            }
        }

        /// <summary>
        /// Seed test yerba mates
        /// </summary>
        /// <param name="factory">Web application factory</param>
        public static async Task SeedTestYerbaMatesAsync(CustomWebApplicationFactory factory)
        {
            var context = GetDbContext(factory);

            if (!await context.YerbaMate.AnyAsync())
            {
                await context.YerbaMate.AddRangeAsync(GetYerbaMates());
                await context.SaveChangesAsync(CancellationToken.None);
            }
        }

        public static async Task SeedTestOpinionsAsync(CustomWebApplicationFactory factory)
        {
            var context = GetDbContext(factory);

            if (!await context.Opinions.AnyAsync())
            {
                await context.Opinions.AddRangeAsync(GetYerbaMateOpinions());
                await context.SaveChangesAsync(CancellationToken.None);
            }
        }
        
        /// <summary>
        /// Seed test shops
        /// </summary>
        /// <param name="factory">Web application factory</param>
        public static async Task SeedTestShopsAsync(CustomWebApplicationFactory factory)
        {
            var context = GetDbContext(factory);

            if (!await context.Shops.AnyAsync())
            {
                await context.Shops.AddRangeAsync(GetShops());
                await context.SaveChangesAsync(CancellationToken.None);
            }
        }
        
        /// <summary>
        /// Seed test shop opinions
        /// </summary>
        /// <param name="factory">Web application factory</param>
        public static async Task SeedTestShopOpinionsAsync(CustomWebApplicationFactory factory)
        {
            var context = GetDbContext(factory);

            if (!await context.ShopOpinions.AnyAsync())
            {
                await context.ShopOpinions.AddRangeAsync(GetShopOpinions());
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

        /// <summary>
        /// Gets test favourites
        /// </summary>
        /// <returns>List of test favourites</returns>
        private static IEnumerable<Favourite> GetFavourites()
        {
            return new List<Favourite>
            {
                new()
                {
                    Id = Guid.Parse("5F07ACEC-B726-4DA8-968D-5088543D6D85"),
                    YerbaMateId = Guid.Parse("3C24EB64-6CA5-4716-9A9A-42654F0EAF43")
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    YerbaMateId = Guid.NewGuid()
                },
                new()
                {
                    Id = Guid.Parse("042C8BB9-311C-49B6-84D1-1E4993038270"),
                    YerbaMateId = Guid.Parse("3C24EB64-6CA5-4716-9A9A-42654F0EAF43")
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    YerbaMateId = Guid.NewGuid()
                }
            };
        }

        /// <summary>
        /// Gets test yerba mates
        /// </summary>
        /// <returns>List of test yerba mates</returns>
        private static IEnumerable<YerbaMate> GetYerbaMates()
        {
            return new List<YerbaMate>
            {
                new()
                {
                    Id = Guid.Parse("3C24EB64-6CA5-4716-9A9A-42654F0EAF43"),
                    BrandId = Guid.NewGuid(),
                    Name = "Kurupi Katuava",
                    Description = "One of the best herbal yerba",
                    imgUrl = "test img url",
                    AveragePrice = 15.21M,
                    NumberOfAddToFav = 0,
                    CategoryId = Guid.NewGuid()
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    BrandId = Guid.NewGuid(),
                    Name = "Test 1",
                    Description = "Description 1",
                    imgUrl = "Test 1 img url",
                    AveragePrice = 20.99M,
                    NumberOfAddToFav = 0,
                    CategoryId = Guid.NewGuid()
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    BrandId = Guid.NewGuid(),
                    Name = "Test 2",
                    Description = "Description 2",
                    imgUrl = "Test 2 img url",
                    AveragePrice = 9.99M,
                    NumberOfAddToFav = 0,
                    CategoryId = Guid.NewGuid()
                }
            };
        }

        /// <summary>
        /// Gets test yerba mate opinions
        /// </summary>
        /// <returns>List of test yerba mate opinions</returns>
        private static IEnumerable<Opinion> GetYerbaMateOpinions()
        {
            return new List<Opinion>
            {
                new()
                {
                    Id = Guid.Parse("EB2BB300-A4FF-486C-AB64-4EF0A7DB527F"),
                    Rate = 10,
                    Comment = "Comment 1",
                    YerbaMateId = Guid.Parse("3C24EB64-6CA5-4716-9A9A-42654F0EAF43")
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Rate = 9,
                    Comment = "Comment 2",
                    YerbaMateId = Guid.NewGuid()
                },
                new()
                {
                    Id = Guid.Parse("E3544051-5179-4181-B1D9-662DF4BE7797"),
                    Rate = 6,
                    Comment = "test",
                    YerbaMateId = Guid.Parse("3C24EB64-6CA5-4716-9A9A-42654F0EAF43")
                },
            };
        }
        
        /// <summary>
        /// Gets test shops
        /// </summary>
        /// <returns>List of test shops</returns>
        private static IEnumerable<Shop> GetShops()
        {
            return new List<Shop>
            {
                new()
                {
                    Id = Guid.Parse("02F73DA0-343F-4520-AEAD-36246FA446F5"),
                    Name = "Matemundo",
                    Description = "Test description 1",
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Test 2",
                    Description = "Test description 2",
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Name = "Test 3",
                    Description = "Test description 3",
                }
            };
        }

        /// <summary>
        /// Gets test shop opinions
        /// </summary>
        /// <returns>List of test shop opinions</returns>
        private static IEnumerable<ShopOpinion> GetShopOpinions()
        {
            return new List<ShopOpinion>
            {
                new()
                {
                    Id = Guid.Parse("A0EDB43D-5195-4458-8C4B-8F6F9FD7E5C9"),
                    Rate = 10,
                    Comment = "Comment 1",
                    ShopId = Guid.Parse("02F73DA0-343F-4520-AEAD-36246FA446F5")
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Rate = 9,
                    Comment = "Comment 2",
                    ShopId = Guid.NewGuid()
                },
                new()
                {
                    Id = Guid.Parse("5887542F-A4BF-4666-88C4-A497E8240C0E"),
                    Rate = 6,
                    Comment = "Super comment 3",
                    ShopId = Guid.Parse("02F73DA0-343F-4520-AEAD-36246FA446F5")
                },
            };
        }
    }
}