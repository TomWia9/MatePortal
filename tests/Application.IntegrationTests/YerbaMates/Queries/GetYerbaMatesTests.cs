using System;
using System.Threading.Tasks;
using Application.Common.Enums;
using Application.IntegrationTests.Helpers;
using Application.YerbaMates.Queries;
using Application.YerbaMates.Queries.GetYerbaMates;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.YerbaMates.Queries;

/// <summary>
///     Get yerba mates tests
/// </summary>
public class GetYerbaMatesTests : IntegrationTest
{
    /// <summary>
    ///     Get yerba mates should return all yerba mates
    /// </summary>
    [Fact]
    public async Task ShouldReturnAllYerbaMates()
    {
        await TestSeeder.SeedTestBrandsAsync(Factory);
        await TestSeeder.SeedTestCategoriesAsync(Factory);
        await TestSeeder.SeedTestYerbaMatesAsync(Factory);
        var response = await Mediator.Send(new GetYerbaMatesQuery(new YerbaMatesQueryParameters()));
        response.Count.Should().Be(3);
    }

    /// <summary>
    ///     Get yerba mates with specified query search should return correct yerba mates
    /// </summary>
    [Fact]
    public async Task GetYerbaMatesWithSpecifiedQuerySearchShouldReturnCorrectYerbaMates()
    {
        await TestSeeder.SeedTestBrandsAsync(Factory);
        await TestSeeder.SeedTestCategoriesAsync(Factory);
        await TestSeeder.SeedTestYerbaMatesAsync(Factory);

        var response = await Mediator.Send(new GetYerbaMatesQuery(new YerbaMatesQueryParameters
        {
            SearchQuery = "herbal"
        }));

        response.Count.Should().Be(1);
        response[0].Name.Should().Be("Kurupi Katuava");
        response[0].Description.Should().Be("One of the best herbal yerba");
    }

    /// <summary>
    ///     Get yerba mates with specified query parameters should return correct yerba mates
    /// </summary>
    [Fact]
    public async Task GetYerbaMatesWithSpecifiedQueryParametersShouldReturnCorrectYerbaMates()
    {
        await TestSeeder.SeedTestBrandsAsync(Factory);
        await TestSeeder.SeedTestCategoriesAsync(Factory);
        await TestSeeder.SeedTestYerbaMatesAsync(Factory);

        var response = await Mediator.Send(new GetYerbaMatesQuery(new YerbaMatesQueryParameters
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
    ///     Get yerba mates with specified sorting should return correct sorted yerba mates
    /// </summary>
    [Fact]
    public async Task GetYerbaMatesWithSpecifiedSortingShouldReturnCorrectSortedYerbaMates()
    {
        await TestSeeder.SeedTestBrandsAsync(Factory);
        await TestSeeder.SeedTestCategoriesAsync(Factory);
        await TestSeeder.SeedTestYerbaMatesAsync(Factory);
        await TestSeeder.SeedTestOpinionsAsync(Factory);

        var response = await Mediator.Send(new GetYerbaMatesQuery(new YerbaMatesQueryParameters
        {
            SortBy = "Opinions",
            SortDirection = SortDirection.Asc
        }));

        response.Count.Should().Be(3);
        response[2].Id.Should().Be(Guid.Parse("3C24EB64-6CA5-4716-9A9A-42654F0EAF43"));
    }
}