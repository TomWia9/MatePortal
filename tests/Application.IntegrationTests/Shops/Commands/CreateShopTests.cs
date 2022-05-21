using System;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.IntegrationTests.Helpers;
using Application.Shops.Commands.CreateShop;
using Application.Shops.Queries;
using Domain.Entities;
using FluentAssertions;
using FluentAssertions.Extensions;
using Xunit;

namespace Application.IntegrationTests.Shops.Commands;

/// <summary>
///     Create shop tests
/// </summary>
public class CreateShopTests : IntegrationTest
{
    /// <summary>
    ///     Create shop should create shop and return shop data transfer object
    /// </summary>
    [Fact]
    public async Task ShouldCreateShopAndReturnShopDto()
    {
        var userId = await AuthHelper.RunAsAdministratorAsync(Factory);

        var command = new CreateShopCommand
        {
            Name = "Test",
            Description = "Test description"
        };

        var expectedResult = new ShopDto
        {
            Name = command.Name,
            Description = command.Description
        };

        var result = await Mediator.Send(command);

        result.Should().BeOfType<ShopDto>();
        result.Should().BeEquivalentTo(expectedResult, x => x.Excluding(y => y.Id));

        var item = await DbHelper.FindAsync<Shop>(Factory, result.Id);

        item.CreatedBy.Should().NotBeNull();
        item.CreatedBy.Should().Be(userId);
        item.Created.Should().BeCloseTo(DateTime.Now, 1.Seconds());
        item.LastModified.Should().BeNull();
        item.LastModifiedBy.Should().BeNull();
    }

    /// <summary>
    ///     Shop should require unique name
    /// </summary>
    [Fact]
    public async Task ShouldRequireUniqueName()
    {
        var command = new CreateShopCommand
        {
            Name = "Test",
            Description = "Test description"
        };

        await Mediator.Send(command);

        await FluentActions.Invoking(() =>
            Mediator.Send(command)).Should().ThrowAsync<ConflictException>();
    }
}