using System.Linq;
using System.Threading.Tasks;
using Application.Categories.Queries.GetCategories;
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
        await TestSeeder.SeedTestCategoriesAsync(_factory);

        var response = await _mediator.Send(new GetCategoriesQuery());
        response.Count().Should().Be(2);
    }
}