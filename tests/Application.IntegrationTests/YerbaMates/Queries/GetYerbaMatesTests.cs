using System;
using System.Threading.Tasks;
using Application.Common.Enums;
using Application.IntegrationTests.Helpers;
using Application.YerbaMates.Queries;
using Application.YerbaMates.Queries.GetYerbaMates;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.YerbaMates.Queries
{
    /// <summary>
    /// Get yerba mates tests
    /// </summary>
    public class GetYerbaMatesTests : IntegrationTest
    {
        /// <summary>
        /// Get yerba mates should return all yerba mates
        /// </summary>
        [Fact]
        public async Task ShouldReturnAllYerbaMates()
        {
            await TestSeeder.SeedTestBrandsAsync(_factory);
            await TestSeeder.SeedTestCategoriesAsync(_factory);
            await TestSeeder.SeedTestYerbaMatesAsync(_factory);
            var response = await _mediator.Send(new GetYerbaMatesQuery(new YerbaMatesQueryParameters()));
            response.Count.Should().Be(3);
        }

        /// <summary>
        /// Get yerba mates with specified query search should return correct yerba mates
        /// </summary>
        [Fact]
        public async Task GetYerbaMatesWithSpecifiedQuerySearchShouldReturnCorrectYerbaMates()
        {
            await TestSeeder.SeedTestBrandsAsync(_factory);
            await TestSeeder.SeedTestCategoriesAsync(_factory);
            await TestSeeder.SeedTestYerbaMatesAsync(_factory);

            var response = await _mediator.Send(new GetYerbaMatesQuery(new YerbaMatesQueryParameters
            {
                SearchQuery = "herbal"
            }));

            response.Count.Should().Be(1);
            response[0].Name.Should().Be("Kurupi Katuava");
            response[0].Description.Should().Be("One of the best herbal yerba");
        }

        /// <summary>
        /// Get yerba mates with specified query parameters should return correct yerba mates
        /// </summary>
        [Fact]
        public async Task GetYerbaMatesWithSpecifiedQueryParametersShouldReturnCorrectYerbaMates()
        {
            await TestSeeder.SeedTestBrandsAsync(_factory);
            await TestSeeder.SeedTestCategoriesAsync(_factory);
            await TestSeeder.SeedTestYerbaMatesAsync(_factory);

            var response = await _mediator.Send(new GetYerbaMatesQuery(new YerbaMatesQueryParameters
            {
                Country = "Argentina",
                MaxPrice = 22M
            }));

            response.Count.Should().Be(1);
            response[0].Name.Should().Be("Test 1");
            response[0].Description.Should().Be("Description 1");
            response[0].Brand.Name.Should().Be("Cruz de malta");
        }

        /// <summary>
        /// Get yerba mates with specified sorting should return correct sorted yerba mates
        /// </summary>
        [Fact]
        public async Task GetYerbaMatesWithSpecifiedSortingShouldReturnCorrectSortedYerbaMates()
        {
            await TestSeeder.SeedTestBrandsAsync(_factory);
            await TestSeeder.SeedTestCategoriesAsync(_factory);
            await TestSeeder.SeedTestYerbaMatesAsync(_factory);
            await TestSeeder.SeedTestOpinionsAsync(_factory);

            var response = await _mediator.Send(new GetYerbaMatesQuery(new YerbaMatesQueryParameters
            {
                SortBy = "Opinions",
                SortDirection = SortDirection.ASC
            }));

            response.Count.Should().Be(3);
            response[2].Id.Should().Be(Guid.Parse("3C24EB64-6CA5-4716-9A9A-42654F0EAF43"));
        }
    }
}