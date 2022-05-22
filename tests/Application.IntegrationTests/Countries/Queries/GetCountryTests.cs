using System;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Countries.Queries;
using Application.Countries.Queries.GetCountry;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.Countries.Queries;

/// <summary>
///     Get country tests
/// </summary>
public class GetCountryTests : IntegrationTest
{
    /// <summary>
    ///     Get country command should return correct country data transfer object
    /// </summary>
    [Fact]
    public async Task ShouldReturnCorrectCountry()
    {
        var countryId = Guid.Parse("A7BBB4DA-12D5-4227-B6BA-690ECF40CD86"); //id of one of seeded country

        var expectedResult = new CountryDto
        {
            Id = countryId,
            Name = "Brazil"
        };

        var response = await Mediator.Send(new GetCountryQuery(countryId));

        response.Should().BeOfType<CountryDto>();
        response.Should().BeEquivalentTo(expectedResult);
    }

    /// <summary>
    ///     Get country with incorrect id should throw not found exception
    /// </summary>
    [Fact]
    public void GetCountryWithIncorrectIdShouldThrowNotFound()
    {
        var countryId = Guid.Empty;

        FluentActions.Invoking(() =>
            Mediator.Send(new GetCountryQuery(countryId))).Should().ThrowAsync<NotFoundException>();
    }
}