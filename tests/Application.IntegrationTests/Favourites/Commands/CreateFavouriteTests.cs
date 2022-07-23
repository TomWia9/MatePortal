using System;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Favourites.Commands.CreateFavourite;
using Application.Favourites.Queries;
using Application.IntegrationTests.Helpers;
using Application.YerbaMates.Queries.GetYerbaMate;
using Domain.Entities;
using FluentAssertions;
using FluentAssertions.Extensions;
using Xunit;

namespace Application.IntegrationTests.Favourites.Commands;

/// <summary>
///     Create favourite tests
/// </summary>
public class CreateFavouriteTests : IntegrationTest
{
    /// <summary>
    ///     Create favourite should create favourite and return favourite data transfer object
    /// </summary>
    [Fact]
    public async Task ShouldCreateFavouriteAndReturnFavouriteDto()
    {
        await TestSeeder.SeedTestYerbaMatesAsync(Factory);

        var userId = await AuthHelper.RunAsDefaultUserAsync(Factory);

        var command = new CreateFavouriteCommand
        {
            YerbaMateId = Guid.Parse("3C24EB64-6CA5-4716-9A9A-42654F0EAF43") //one of seeded yerba mate
        };

        var result = await Mediator.Send(command);

        var item = await DbHelper.FindAsync<Favourite>(Factory, result.Id);

        result.Should().BeOfType<FavouriteDto>();
        result.YerbaMateId.Should().Be(command.YerbaMateId);

        item.CreatedBy.Should().NotBeNull();
        item.CreatedBy.Should().Be(userId);
        item.Created.Should().BeCloseTo(DateTime.Now, 1.Seconds());
        item.LastModified.Should().BeNull();
        item.LastModifiedBy.Should().BeNull();
    }

    /// <summary>
    ///     Should increase yerba mate number of additions to favourites
    /// </summary>
    [Fact]
    public async Task ShouldIncreaseYerbaMateNumberOfAddToFav()
    {
        await TestSeeder.SeedTestBrandsAsync(Factory);
        await TestSeeder.SeedTestCategoriesAsync(Factory);
        await TestSeeder.SeedTestYerbaMatesAsync(Factory);

        await AuthHelper.RunAsDefaultUserAsync(Factory);

        var command = new CreateFavouriteCommand
        {
            YerbaMateId = Guid.Parse("3C24EB64-6CA5-4716-9A9A-42654F0EAF43") //one of seeded yerba mate
        };

        await Mediator.Send(command);

        var yerbaMateDto = await Mediator.Send(new GetYerbaMateQuery(command.YerbaMateId));
        yerbaMateDto.NumberOfAddToFav.Should().Be(1);
    }

    /// <summary>
    ///     Create favourite for nonexistent yerba should throw NotFound
    /// </summary>
    [Fact]
    public async Task CreateFavouriteForNonexistentYerbaMateShouldThrowNotFound()
    {
        await AuthHelper.RunAsDefaultUserAsync(Factory);
        var yerbaMateId = Guid.NewGuid();

        var command = new CreateFavouriteCommand
        {
            YerbaMateId = yerbaMateId
        };

        await FluentActions.Invoking(() =>
            Mediator.Send(command)).Should().ThrowAsync<NotFoundException>();
    }

    /// <summary>
    ///     Favourite should not be added more than once to one item by one user
    /// </summary>
    [Fact]
    public async Task FavouriteShouldNotBeAddedMoreThanOnceToOneItemByOneUser()
    {
        await TestSeeder.SeedTestYerbaMatesAsync(Factory);

        await AuthHelper.RunAsDefaultUserAsync(Factory);
        var yerbaMateId = Guid.Parse("3C24EB64-6CA5-4716-9A9A-42654F0EAF43"); //id of one of seeded yerba mate

        await Mediator.Send(new CreateFavouriteCommand
        {
            YerbaMateId = yerbaMateId
        });

        var command = new CreateFavouriteCommand
        {
            YerbaMateId = yerbaMateId
        };

        await FluentActions.Invoking(() =>
            Mediator.Send(command)).Should().ThrowAsync<ConflictException>();
    }
}