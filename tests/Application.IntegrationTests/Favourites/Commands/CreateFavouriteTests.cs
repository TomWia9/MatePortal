﻿using System;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Favourites.Commands.CreateFavourite;
using Application.Favourites.Queries;
using Application.IntegrationTests.Helpers;
using Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.Favourites.Commands
{
    /// <summary>
    /// Create favourite tests
    /// </summary>
    public class CreateFavouriteTests : IntegrationTest
    {
        /// <summary>
        /// Create favourite should create favourite and return favourite data transfer object
        /// </summary>
        [Fact]
        public async Task ShouldCreateFavouriteAndReturnFavouriteDto()
        {
            await TestSeeder.SeedTestYerbaMatesAsync(_factory);

            var userId = await AuthHelper.RunAsDefaultUserAsync(_factory);

            var command = new CreateFavouriteCommand()
            {
                UserId = userId,
                YerbaMateId = Guid.Parse("3C24EB64-6CA5-4716-9A9A-42654F0EAF43") //one of seeded yerba mate
            };

            var result = await _mediator.Send(command);

            var item = await DbHelper.FindAsync<Favourite>(_factory, result.Id);

            result.Should().BeOfType<FavouriteDto>();
            result.YerbaMateId.Should().Be(command.YerbaMateId);

            item.CreatedBy.Should().NotBeNull();
            item.CreatedBy.Should().Be(userId);
            item.Created.Should().BeCloseTo(DateTime.Now, 1000);
            item.LastModified.Should().BeNull();
            item.LastModifiedBy.Should().BeNull();
        }

        //Todo Create should increase yerba numberOfAddToFav property

        /// <summary>
        /// Create favourite for nonexistent yerba should throw NotFound
        /// </summary>
        [Fact]
        public async Task CreateFavouriteForNonexistentYerbaMateShouldThrowNotFound()
        {
            var userId = await AuthHelper.RunAsDefaultUserAsync(_factory);
            var yerbaMateId = Guid.NewGuid();

            var command = new CreateFavouriteCommand()
            {
                UserId = userId,
                YerbaMateId = yerbaMateId
            };

            FluentActions.Invoking(() =>
                _mediator.Send(command)).Should().Throw<NotFoundException>();
        }

        /// <summary>
        /// Favourite should not be added more than once to one item by one user
        /// </summary>
        [Fact]
        public async Task FavouriteShouldNotBeAddedMoreThanOnceToOneItemByOneUser()
        {
            await TestSeeder.SeedTestYerbaMatesAsync(_factory);

            var userId = await AuthHelper.RunAsDefaultUserAsync(_factory);
            var yerbaMateId = Guid.Parse("3C24EB64-6CA5-4716-9A9A-42654F0EAF43"); //id of one of seeded yerba mate

            await _mediator.Send(new CreateFavouriteCommand()
            {
                UserId = userId,
                YerbaMateId = yerbaMateId
            });

            var command = new CreateFavouriteCommand()
            {
                UserId = userId,
                YerbaMateId = yerbaMateId
            };

            FluentActions.Invoking(() =>
                _mediator.Send(command)).Should().Throw<ConflictException>();
        }
    }
}