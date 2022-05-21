using System;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Countries.Commands.DeleteCountry;
using Application.IntegrationTests.Helpers;
using Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.Countries.Commands;

/// <summary>
///     Delete country tests
/// </summary>
public class DeleteCountryTests : IntegrationTest
{
    /// <summary>
    ///     Delete country with incorrect id should throw not found exception
    /// </summary>
    [Fact]
    public void DeleteCountryWithIncorrectIdShouldThrowNotFound()
    {
        var countryId = Guid.Empty;

        FluentActions.Invoking(() =>
            _mediator.Send(new DeleteCountryCommand {CountryId = countryId})).Should().ThrowAsync<NotFoundException>();
    }

    /// <summary>
    ///     Delete country command should delete country
    /// </summary>
    [Fact]
    public async Task ShouldDeleteCountry()
    {
        var countryId = Guid.Parse("A7BBB4DA-12D5-4227-B6BA-690ECF40CD86"); //one of seeded countries

        //delete
        await _mediator.Send(new DeleteCountryCommand {CountryId = countryId});

        //Assert that deleted
        var item = await DbHelper.FindAsync<Country>(_factory, countryId);
        item.Should().BeNull();
    }
}