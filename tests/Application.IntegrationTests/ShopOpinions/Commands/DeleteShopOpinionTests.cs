﻿using System;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.IntegrationTests.Helpers;
using Application.ShopOpinions.Commands.CreateShopOpinion;
using Application.ShopOpinions.Commands.DeleteShopOpinion;
using Application.Shops.Queries.GetShop;
using Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.ShopOpinions.Commands;

/// <summary>
///     Delete shop opinion tests
/// </summary>
public class DeleteShopOpinionTests : IntegrationTest
{
    /// <summary>
    ///     Delete shop opinion with incorrect id should throw not found exception
    /// </summary>
    [Fact]
    public void DeleteShopOpinionWithIncorrectIdShouldThrowNotFound()
    {
        FluentActions.Invoking(() =>
                Mediator.Send(new DeleteShopOpinionCommand {ShopOpinionId = Guid.Empty})).Should()
            .ThrowAsync<NotFoundException>();
    }

    /// <summary>
    ///     Delete shop opinion command should delete shop opinion
    /// </summary>
    [Fact]
    public async Task ShouldDeleteFavourite()
    {
        await TestSeeder.SeedTestShopsAsync(Factory);
        await AuthHelper.RunAsDefaultUserAsync(Factory);

        //create shop opinion firstly
        var shopOpinionToDeleteDto = await Mediator.Send(new CreateShopOpinionCommand
        {
            Rate = 10,
            Comment = "test",
            ShopId = Guid.Parse("02F73DA0-343F-4520-AEAD-36246FA446F5") //id of one of seeded shops
        });

        //delete
        await Mediator.Send(new DeleteShopOpinionCommand {ShopOpinionId = shopOpinionToDeleteDto.Id});

        //Assert that deleted
        var item = await DbHelper.FindAsync<ShopOpinion>(Factory, shopOpinionToDeleteDto.Id);
        item.Should().BeNull();
    }

    /// <summary>
    ///     User should not be able to delete other user shop opinion
    /// </summary>
    [Fact]
    public async Task UserShouldNotBeAbleToDeleteOtherUserShopOpinion()
    {
        await TestSeeder.SeedTestShopsAsync(Factory);
        await AuthHelper.RunAsDefaultUserAsync(Factory);

        //create shop opinion firstly
        var ShopOpinionToDeleteDto = await Mediator.Send(new CreateShopOpinionCommand
        {
            Rate = 10,
            Comment = "test",
            ShopId = Guid.Parse("02F73DA0-343F-4520-AEAD-36246FA446F5") //id of one of seeded shops
        });

        //change user
        Factory.CurrentUserId = Guid.NewGuid();

        //delete
        await FluentActions.Invoking(() =>
                Mediator.Send(new DeleteShopOpinionCommand {ShopOpinionId = ShopOpinionToDeleteDto.Id}))
            .Should()
            .ThrowAsync<ForbiddenAccessException>();
    }

    /// <summary>
    ///     Administrator should be able to delete user shop opinion
    /// </summary>
    [Fact]
    public async Task AdministratorShouldBeAbleToDeleteUserShopOpinion()
    {
        await AuthHelper.RunAsDefaultUserAsync(Factory);
        await TestSeeder.SeedTestShopsAsync(Factory);

        //create shop opinion firstly
        var shopOpinionToDeleteDto = await Mediator.Send(new CreateShopOpinionCommand
        {
            Rate = 10,
            Comment = "test",
            ShopId = Guid.Parse("02F73DA0-343F-4520-AEAD-36246FA446F5") //id of one of seeded shops
        });

        await AuthHelper
            .RunAsAdministratorAsync(Factory);

        //delete
        await Mediator.Send(new DeleteShopOpinionCommand {ShopOpinionId = shopOpinionToDeleteDto.Id});

        //Assert that deleted
        var item = await DbHelper.FindAsync<ShopOpinion>(Factory, shopOpinionToDeleteDto.Id);
        item.Should().BeNull();
    }


    /// <summary>
    ///     Delete should decrease shop number of opinions
    /// </summary>
    [Fact]
    public async Task ShouldDecreaseShopNumberOfOpinions()
    {
        await TestSeeder.SeedTestShopsAsync(Factory);
        await AuthHelper.RunAsDefaultUserAsync(Factory);

        var command = new CreateShopOpinionCommand
        {
            Comment = "Test",
            Rate = 8,
            ShopId = Guid.Parse("02F73DA0-343F-4520-AEAD-36246FA446F5") //one of seeded shop
        };

        var shopOpinionToDeleteDto = await Mediator.Send(command);

        //delete
        await Mediator.Send(new DeleteShopOpinionCommand {ShopOpinionId = shopOpinionToDeleteDto.Id});

        var shopDto = await Mediator.Send(new GetShopQuery(command.ShopId));
        shopDto.NumberOfShopOpinions.Should().Be(0);
    }
}