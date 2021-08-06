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

namespace Application.IntegrationTests.Favourites.Commands
{
    /// <summary>
    /// Delete favourite tests
    /// </summary>
    public class DeleteFavouriteTests : IntegrationTest
    {
        /// <summary>
        /// Delete favourite with incorrect id should throw not found exception
        /// </summary>
        [Fact]
        public void DeleteFavouriteWithIncorrectIdShouldThrowNotFound()
        {
            var favouriteId = Guid.Empty;

            FluentActions.Invoking(() =>
                    _mediator.Send(new DeleteFavouriteCommand() { FavouriteId = favouriteId })).Should()
                .Throw<NotFoundException>();
        }

        /// <summary>
        /// Delete favourite command should delete favourite
        /// </summary>
        [Fact]
        public async Task ShouldDeleteFavourite()
        {
            await TestSeeder.SeedTestYerbaMatesAsync(_factory);
            var userId = await AuthHelper.RunAsDefaultUserAsync(_factory);

            //create favourite firstly
            var favouriteToDeleteDto = await _mediator.Send(new CreateFavouriteCommand()
            {
                YerbaMateId = Guid.Parse("3C24EB64-6CA5-4716-9A9A-42654F0EAF43") //id of one of seeded yerba mate
            });

            //delete
            await _mediator.Send(new DeleteFavouriteCommand() { FavouriteId = favouriteToDeleteDto.Id });

            //Assert that deleted
            var item = await DbHelper.FindAsync<Favourite>(_factory, favouriteToDeleteDto.Id);
            item.Should().BeNull();
        }

        /// <summary>
        /// Should decrease yerba mate number of additions to favourites
        /// </summary>
        [Fact]
        public async Task ShouldDecreaseYerbaMateNumberOfAddToFav()
        {
            await TestSeeder.SeedTestBrandsAsync(_factory);
            await TestSeeder.SeedTestCategoriesAsync(_factory);
            await TestSeeder.SeedTestYerbaMatesAsync(_factory);

            await AuthHelper.RunAsDefaultUserAsync(_factory);

            var command = new CreateFavouriteCommand()
            {
                YerbaMateId = Guid.Parse("3C24EB64-6CA5-4716-9A9A-42654F0EAF43") //one of seeded yerba mate
            };

            var favouriteToDeleteDto = await _mediator.Send(command);

            //delete
            await _mediator.Send(new DeleteFavouriteCommand() { FavouriteId = favouriteToDeleteDto.Id });

            var yerbaMateDto = await _mediator.Send(new GetYerbaMateQuery(command.YerbaMateId));
            yerbaMateDto.NumberOfAddToFav.Should().Be(0);
        }

        /// <summary>
        /// User should not be able to delete other user favourite
        /// </summary>
        [Fact]
        public async Task UserShouldNotBeAbleToDeleteOtherUserFavourite()
        {
            await TestSeeder.SeedTestYerbaMatesAsync(_factory);
            await AuthHelper.RunAsDefaultUserAsync(_factory);

            //create favourite firstly
            var favouriteToDeleteDto = await _mediator.Send(new CreateFavouriteCommand()
            {
                YerbaMateId = Guid.Parse("3C24EB64-6CA5-4716-9A9A-42654F0EAF43") //id of one of seeded yerba mate
            });

            _factory.CurrentUserId = Guid.NewGuid(); //other user

            //delete
            FluentActions.Invoking(() =>
                    _mediator.Send(new DeleteFavouriteCommand() { FavouriteId = favouriteToDeleteDto.Id })).Should()
                .Throw<ForbiddenAccessException>();
        }

        /// <summary>
        /// Administrator should be able to delete user favourite
        /// </summary>
        [Fact]
        public async Task AdministratorShouldBeAbleToDeleteUserFavourite()
        {
            await AuthHelper.RunAsDefaultUserAsync(_factory);
            await TestSeeder.SeedTestYerbaMatesAsync(_factory);

            //create favourite firstly
            var favouriteToDeleteDto = await _mediator.Send(new CreateFavouriteCommand()
            {
                YerbaMateId = Guid.Parse("3C24EB64-6CA5-4716-9A9A-42654F0EAF43") //id of one of seeded yerba mate
            });

            await AuthHelper.RunAsAdministratorAsync(_factory);

            //delete
            await _mediator.Send(new DeleteFavouriteCommand() { FavouriteId = favouriteToDeleteDto.Id });

            //Assert that deleted
            var item = await DbHelper.FindAsync<Favourite>(_factory, favouriteToDeleteDto.Id);
            item.Should().BeNull();
        }
    }
}