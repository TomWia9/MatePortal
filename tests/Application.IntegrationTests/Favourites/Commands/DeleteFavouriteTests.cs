using System;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Favourites.Commands.CreateFavourite;
using Application.Favourites.Commands.DeleteFavourite;
using Application.IntegrationTests.Helpers;
using Application.YerbaMates.Queries.GetYerbaMate;
using Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.Favourites.Commands;

/// <summary>
///     Delete favourite tests
/// </summary>
public class DeleteFavouriteTests : IntegrationTest
{
    /// <summary>
    ///     Delete favourite with incorrect id should throw not found exception
    /// </summary>
    [Fact]
    public void DeleteFavouriteWithIncorrectIdShouldThrowNotFound()
    {
        var favouriteId = Guid.Empty;

        FluentActions.Invoking(() =>
                Mediator.Send(new DeleteFavouriteCommand {FavouriteId = favouriteId})).Should()
            .ThrowAsync<NotFoundException>();
    }

    /// <summary>
    ///     Delete favourite command should delete favourite
    /// </summary>
    [Fact]
    public async Task ShouldDeleteFavourite()
    {
        await TestSeeder.SeedTestYerbaMatesAsync(Factory);
        var userId = await AuthHelper.RunAsDefaultUserAsync(Factory);

        //create favourite firstly
        var favouriteToDeleteDto = await Mediator.Send(new CreateFavouriteCommand
        {
            YerbaMateId = Guid.Parse("3C24EB64-6CA5-4716-9A9A-42654F0EAF43") //id of one of seeded yerba mate
        });

        //delete
        await Mediator.Send(new DeleteFavouriteCommand {FavouriteId = favouriteToDeleteDto.Id});

        //Assert that deleted
        var item = await DbHelper.FindAsync<Favourite>(Factory, favouriteToDeleteDto.Id);
        item.Should().BeNull();
    }

    /// <summary>
    ///     Should decrease yerba mate number of additions to favourites
    /// </summary>
    [Fact]
    public async Task ShouldDecreaseYerbaMateNumberOfAddToFav()
    {
        await TestSeeder.SeedTestBrandsAsync(Factory);
        await TestSeeder.SeedTestCategoriesAsync(Factory);
        await TestSeeder.SeedTestYerbaMatesAsync(Factory);

        await AuthHelper.RunAsDefaultUserAsync(Factory);

        var command = new CreateFavouriteCommand
        {
            YerbaMateId = Guid.Parse("3C24EB64-6CA5-4716-9A9A-42654F0EAF43") //one of seeded yerba mate
        };

        var favouriteToDeleteDto = await Mediator.Send(command);

        //delete
        await Mediator.Send(new DeleteFavouriteCommand {FavouriteId = favouriteToDeleteDto.Id});

        var yerbaMateDto = await Mediator.Send(new GetYerbaMateQuery(command.YerbaMateId));
        yerbaMateDto.NumberOfAddToFav.Should().Be(0);
    }

    /// <summary>
    ///     User should not be able to delete other user favourite
    /// </summary>
    [Fact]
    public async Task UserShouldNotBeAbleToDeleteOtherUserFavourite()
    {
        await TestSeeder.SeedTestYerbaMatesAsync(Factory);
        await AuthHelper.RunAsDefaultUserAsync(Factory);

        //create favourite firstly
        var favouriteToDeleteDto = await Mediator.Send(new CreateFavouriteCommand
        {
            YerbaMateId = Guid.Parse("3C24EB64-6CA5-4716-9A9A-42654F0EAF43") //id of one of seeded yerba mate
        });

        Factory.CurrentUserId = Guid.NewGuid(); //other user

        //delete
        await FluentActions.Invoking(() =>
                Mediator.Send(new DeleteFavouriteCommand {FavouriteId = favouriteToDeleteDto.Id})).Should()
            .ThrowAsync<ForbiddenAccessException>();
    }

    /// <summary>
    ///     Administrator should be able to delete user favourite
    /// </summary>
    [Fact]
    public async Task AdministratorShouldBeAbleToDeleteUserFavourite()
    {
        await AuthHelper.RunAsDefaultUserAsync(Factory);
        await TestSeeder.SeedTestYerbaMatesAsync(Factory);

        //create favourite firstly
        var favouriteToDeleteDto = await Mediator.Send(new CreateFavouriteCommand
        {
            YerbaMateId = Guid.Parse("3C24EB64-6CA5-4716-9A9A-42654F0EAF43") //id of one of seeded yerba mate
        });

        await AuthHelper.RunAsAdministratorAsync(Factory);

        //delete
        await Mediator.Send(new DeleteFavouriteCommand {FavouriteId = favouriteToDeleteDto.Id});

        //Assert that deleted
        var item = await DbHelper.FindAsync<Favourite>(Factory, favouriteToDeleteDto.Id);
        item.Should().BeNull();
    }
}