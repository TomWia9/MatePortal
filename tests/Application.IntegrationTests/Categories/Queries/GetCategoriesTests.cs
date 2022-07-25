using System.Threading.Tasks;
using Application.Categories.Queries;
using Application.Categories.Queries.GetCategories;
using Application.Common.Enums;
using Application.IntegrationTests.Helpers;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.Categories.Queries;

/// <summary>
///     Get categories tests
/// </summary>
public class GetCategories : IntegrationTest
{
    /// <summary>
    ///     Get categories should return all categories
    /// </summary>
    [Fact]
    public async Task ShouldReturnAllCategories()
    {
        await TestSeeder.SeedTestCategoriesAsync(Factory);

        var response = await Mediator.Send(new GetCategoriesQuery(new CategoriesQueryParameters()));
        response.Count.Should().Be(2);
    }

    /// <summary>
    ///     Get categories should return correct categories when search query specified
    /// </summary>
    [Fact]
    public async Task ShouldReturnCorrectCategoriesWhenSearchQuerySpecified()
    {
        await TestSeeder.SeedTestCategoriesAsync(Factory);

        var parameters = new CategoriesQueryParameters()
        {
            SearchQuery = "herbal"
        };

        var response = await Mediator.Send(new GetCategoriesQuery(parameters));
        response.Count.Should().Be(1);
        response[0].Name.Should().Be("Herbal");
    }

    /// <summary>
    ///     Get categories should return categories in correct order when sort by specified
    /// </summary>
    [Fact]
    public async Task ShouldReturnCategoriesInCorrectOrderWhenSortBySpecified()
    {
        await TestSeeder.SeedTestCategoriesAsync(Factory);

        var parameters = new CategoriesQueryParameters()
        {
            SortBy = "name",
            SortDirection = SortDirection.Desc
        };

        var response = await Mediator.Send(new GetCategoriesQuery(parameters));
        response.Count.Should().Be(2);
        response[0].Name.Should().Be("Herbal");
        response[1].Name.Should().Be("Fruit");
    }
}