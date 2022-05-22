using System;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.IntegrationTests.Helpers;
using Application.Shops.Commands.UpdateShop;
using Domain.Entities;
using FluentAssertions;
using FluentAssertions.Extensions;
using Xunit;

namespace Application.IntegrationTests.Shops.Commands;

/// <summary>
///     Update shop tests
/// </summary>
public class UpdateShopTests : IntegrationTest
{
    /// <summary>
    ///     Update shop with incorrect id should throw not found exception
    /// </summary>
    [Fact]
    public void UpdateShopWithIncorrectIdShouldThrowNotFound()
    {
        var shopId = Guid.Empty;

        var command = new UpdateShopCommand
        {
            ShopId = shopId,
            Name = "Test",
            Description = "Test description"
        };

        FluentActions.Invoking(() =>
            Mediator.Send(command)).Should().ThrowAsync<NotFoundException>();
    }

    /// <summary>
    ///     Update shop command should update shop
    /// </summary>
    [Fact]
    public async Task UpdateShopShouldUpdateShop()
    {
        var userId = await AuthHelper.RunAsAdministratorAsync(Factory);

        await TestSeeder.SeedTestShopsAsync(Factory);

        var shopId = Guid.Parse("02F73DA0-343F-4520-AEAD-36246FA446F5"); //one of seeded shops

        var command = new UpdateShopCommand
        {
            ShopId = shopId,
            Name = "Updated shop name",
            Description = "Updated description"
        };

        await Mediator.Send(command);

        var item = await DbHelper.FindAsync<Shop>(Factory, shopId);

        item.Name.Should().Be(command.Name);
        item.Description.Should().Be(command.Description);
        item.CreatedBy.Should().NotBeNull();
        item.LastModifiedBy.Should().NotBeNull();
        item.LastModifiedBy.Should().Be(userId);
        item.LastModified.Should().NotBeNull();
        item.LastModified.Should().BeCloseTo(DateTime.Now, 1.Seconds());
    }
}