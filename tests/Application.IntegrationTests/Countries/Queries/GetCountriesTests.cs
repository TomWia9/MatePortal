using System.Threading.Tasks;
using Application.Common.Enums;
using Application.Countries.Queries;
using Application.Countries.Queries.GetCountries;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.Countries.Queries;

/// <summary>
///     Get countries tests
/// </summary>
public class GetCountries : IntegrationTest
{
    /// <summary>
    ///     Get countries should return 4 countries
    /// </summary>
    [Fact]
    public async Task ShouldReturn4Countries()
    {
        var response = await Mediator.Send(new GetCountriesQuery(new CountriesQueryParameters()));
        response.Count.Should().Be(4);
    }

    /// <summary>
    ///     Get countries should return correct countries when search query specified
    /// </summary>
    [Fact]
    public async Task ShouldReturnCorrectCountriesWhenSearchQuerySpecified()
    {
        var parameters = new CountriesQueryParameters
        {
            SearchQuery = "arg"
        };

        var response = await Mediator.Send(new GetCountriesQuery(parameters));
        response.Count.Should().Be(1);
        response[0].Name.Should().Be("Argentina");
    }

    /// <summary>
    ///     Get countries should return countries in correct order when sort by specified
    /// </summary>
    [Fact]
    public async Task ShouldReturnCountriesInCorrectOrderWhenSortBySpecified()
    {
        var parameters = new CountriesQueryParameters
        {
            SortBy = "name",
            SortDirection = SortDirection.Desc
        };

        var response = await Mediator.Send(new GetCountriesQuery(parameters));
        response.Count.Should().Be(4);
        response[0].Name.Should().Be("Uruguay");
        response[1].Name.Should().Be("Paraguay");
        response[2].Name.Should().Be("Brazil");
        response[3].Name.Should().Be("Argentina");
    }
}