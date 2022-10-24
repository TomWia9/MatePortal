using System;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.IntegrationTests.Helpers;
using Application.ShopOpinions.Commands.CreateShopOpinion;
using Application.ShopOpinions.Commands.UpdateShopOpinion;
using Domain.Entities;
using FluentAssertions;
using FluentAssertions.Extensions;
using Xunit;

namespace Application.IntegrationTests.ShopOpinions.Commands;

/// <summary>
///     Update shop opinion tests
/// </summary>
public class UpdateShopOpinionTests : IntegrationTest
{
    /// <summary>
    ///     Update shop opinion with incorrect id should throw not found exception
    /// </summary>
    [Fact]
    public void UpdateShopOpinionWithIncorrectIdShouldThrowNotFound()
    {
        var command = new UpdateShopOpinionCommand
        {
            ShopOpinionId = Guid.Empty,
            Rate = 2,
            Comment = "Updated comment"
        };

        FluentActions.Invoking(() =>
            Mediator.Send(command)).Should().ThrowAsync<NotFoundException>();
    }

    /// <summary>
    ///     Update shop opinion command should update shop opinion
    /// </summary>
    [Fact]
    public async Task UpdateShopOpinionShouldUpdateShopOpinion()
    {
        var userId = await AuthHelper.RunAsDefaultUserAsync(Factory);
        await TestSeeder.SeedTestShopOpinionsAsync(Factory);

        var shopOpinionId = Guid.Parse("A0EDB43D-5195-4458-8C4B-8F6F9FD7E5C9"); //one of seeded shop opinions

        var command = new UpdateShopOpinionCommand
        {
            ShopOpinionId = shopOpinionId,
            Rate = 4,
            Comment = "Updated comment"
        };

        await Mediator.Send(command);

        var item = await DbHelper.FindAsync<ShopOpinion>(Factory, shopOpinionId);

        item.Rate.Should().Be(command.Rate);
        item.Comment.Should().Be(command.Comment);
        item.CreatedBy.Should().NotBeNull();
        item.LastModified.Should().NotBeNull();
        item.LastModified.Should().BeCloseTo(DateTime.Now, 1.Seconds());
        item.LastModifiedBy.Should().NotBeNull();
        item.LastModifiedBy.Should().Be(userId);
    }

    /// <summary>
    ///     User should not be able to update other user shop opinion
    /// </summary>
    [Fact]
    public async Task UserShouldNotBeAbleToUpdateOtherUserShopOpinion()
    {
        await TestSeeder.SeedTestShopsAsync(Factory);
        await AuthHelper.RunAsDefaultUserAsync(Factory);

        //create shop opinion firstly
        var shopOpinionToUpdateDto = await Mediator.Send(new CreateShopOpinionCommand
        {
            Rate = 10,
            Comment = "test",
            ShopId = Guid.Parse("02F73DA0-343F-4520-AEAD-36246FA446F5") //id of one of seeded shops
        });

        //change user
        Factory.CurrentUserId = Guid.NewGuid();

        await FluentActions.Invoking(() =>
                Mediator.Send(new UpdateShopOpinionCommand
                    {ShopOpinionId = shopOpinionToUpdateDto.Id, Comment = "test", Rate = 1})).Should()
            .ThrowAsync<ForbiddenAccessException>();
    }

    /// <summary>
    ///     Administrator should be able to update user shop opinion
    /// </summary>
    [Fact]
    public async Task AdministratorShouldBeAbleToUpdateUserShopOpinion()
    {
        await AuthHelper.RunAsDefaultUserAsync(Factory);
        await TestSeeder.SeedTestShopsAsync(Factory);

        //create shop opinion firstly
        var shopOpinionToUpdateDto = await Mediator.Send(new CreateShopOpinionCommand
        {
            Rate = 10,
            Comment = "test",
            ShopId = Guid.Parse("02F73DA0-343F-4520-AEAD-36246FA446F5") //id of one of seeded shops
        });

        //change user
        await AuthHelper.RunAsAdministratorAsync(Factory);

        var command = new UpdateShopOpinionCommand
        {
            ShopOpinionId = shopOpinionToUpdateDto.Id,
            Comment = "test1",
            Rate = 2
        };

        await Mediator.Send(command);

        //Assert that updated
        var item = await DbHelper.FindAsync<ShopOpinion>(Factory, shopOpinionToUpdateDto.Id);
        item.Rate.Should().Be(command.Rate);
        item.Comment.Should().Be(command.Comment);
        item.CreatedBy.Should().NotBeNull();
        item.LastModified.Should().BeCloseTo(DateTime.Now, 1.Seconds());
        item.LastModifiedBy.Should().NotBeNull();
    }
}