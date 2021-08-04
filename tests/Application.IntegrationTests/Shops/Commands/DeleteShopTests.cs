using System;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.IntegrationTests.Helpers;
using Application.Shops.Commands.DeleteShop;
using Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.Shops.Commands
{
    /// <summary>
    /// Delete shop tests
    /// </summary>
    public class DeleteShopTests : IntegrationTest
    {
        /// <summary>
        /// Delete shop with incorrect id should throw not found exception
        /// </summary>
        [Fact]
        public void DeleteShopWithIncorrectIdShouldThrowNotFound()
        {
            FluentActions.Invoking(() =>
                _mediator.Send(new DeleteShopCommand() {ShopId = Guid.Empty})).Should().Throw<NotFoundException>();
        }

        /// <summary>
        /// Delete shop command should delete shop
        /// </summary>
        [Fact]
        public async Task ShouldDeleteShop()
        {
            await TestSeeder.SeedTestShopsAsync(_factory);

            var shopId = Guid.Parse("02F73DA0-343F-4520-AEAD-36246FA446F5"); //one of seeded shops

            //delete
            await _mediator.Send(new DeleteShopCommand{ShopId = shopId});
            
            //Assert that deleted
            var item = await DbHelper.FindAsync<Shop>(_factory, shopId);
            item.Should().BeNull();
        }
    }
}