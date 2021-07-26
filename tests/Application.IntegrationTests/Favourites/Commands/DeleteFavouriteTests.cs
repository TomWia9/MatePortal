﻿using System;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Favourites.Commands.CreateFavourite;
using Application.Favourites.Commands.DeleteFavourite;
using Application.IntegrationTests.Helpers;
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
                    _mediator.Send(new DeleteFavouriteCommand() {FavouriteId = favouriteId})).Should()
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
                UserId = userId,
                YerbaMateId = Guid.Parse("3C24EB64-6CA5-4716-9A9A-42654F0EAF43") //id of one of seeded yerba mate
            });

            //delete
            await _mediator.Send(new DeleteFavouriteCommand() {FavouriteId = favouriteToDeleteDto.Id});

            //Assert that deleted
            var item = await DbHelper.FindAsync<Favourite>(_factory, favouriteToDeleteDto.Id);
            item.Should().BeNull();
        }

        //TODO Delete should decrease yerba numberOfAddToFav property
    }
}