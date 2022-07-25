using System;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Countries.Commands.UpdateCountry;
using Application.IntegrationTests.Helpers;
using Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.Countries.Commands;

/// <summary>
///     Update country tests
/// </summary>
public class UpdateCountryTests : IntegrationTest
{
    /// <summary>
    ///     Update country with incorrect id should throw not found exception
    /// </summary>
    [Fact]
    public void UpdateCountryWithIncorrectIdShouldThrowNotFound()
    {
        var countryId = Guid.Empty;

        var command = new UpdateCountryCommand
        {
            CountryId = countryId,
            Name = "test"
        };

        FluentActions.Invoking(() =>
            Mediator.Send(command)).Should().ThrowAsync<NotFoundException>();
    }

    /// <summary>
    ///     Update country command should update country
    /// </summary>
    [Fact]
    public async Task UpdateCountryShouldUpdateCountry()
    {
        var countryId = Guid.Parse("A7BBB4DA-12D5-4227-B6BA-690ECF40CD86"); //one of seeded countries

        var command = new UpdateCountryCommand
        {
            CountryId = countryId,
            Name = "Updated country",
            Description = "Updated description"
        };

        await Mediator.Send(command);

        var item = await DbHelper.FindAsync<Country>(Factory, countryId);

        item.Name.Should().Be(command.Name);
        item.Description.Should().Be(command.Description);
    }
    
    /// <summary>
    ///     Country update should require unique name
    /// </summary>
    [Fact]
    public async Task ShouldRequireUniqueNameWhenUpdating()
    {
        var command = new UpdateCountryCommand()
        {
            CountryId = Guid.Parse("A42066F2-2998-47DC-A193-FF4C4080056F"), //one of seeded countries
            Name = "Argentina", //already exists
            Description = "Test"
        };

        await FluentActions.Invoking(() =>
            Mediator.Send(command)).Should().ThrowAsync<ConflictException>();
    }
    
}