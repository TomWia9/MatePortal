using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Countries.Commands.CreateCountry;
using Application.Countries.Queries;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.Countries.Commands;

/// <summary>
///     Create country tests
/// </summary>
public class CreateCountryTests : IntegrationTest
{
    /// <summary>
    ///     Create country should create country and return country data transfer object
    /// </summary>
    [Fact]
    public async Task ShouldCreateCountryAndReturnCountryDto()
    {
        var command = new CreateCountryCommand
        {
            Name = "Test country"
        };

        var expectedResult = new CountryDto
        {
            Name = command.Name
        };

        var result = await Mediator.Send(command);

        result.Should().BeOfType<CountryDto>();
        result.Should().BeEquivalentTo(expectedResult, x => x.Excluding(y => y.Id));
    }

    /// <summary>
    ///     Country should require unique name
    /// </summary>
    [Fact]
    public async Task ShouldRequireUniqueName()
    {
        await Mediator.Send(new CreateCountryCommand
        {
            Name = "Test"
        });

        var command = new CreateCountryCommand
        {
            Name = "Test"
        };

        await FluentActions.Invoking(() =>
            Mediator.Send(command)).Should().ThrowAsync<ConflictException>();
    }
}