using System;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.IntegrationTests.Helpers;
using Application.YerbaMates.Commands.DeleteYerbaMate;
using Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.YerbaMates.Commands
{
    /// <summary>
    ///     Delete yerba mate tests
    /// </summary>
    public class DeleteYerbaMateTests : IntegrationTest
    {
        /// <summary>
        ///     Delete yerba mate with incorrect id should throw not found exception
        /// </summary>
        [Fact]
        public void DeleteYerbaMateWithIncorrectIdShouldThrowNotFound()
        {
            FluentActions.Invoking(() =>
                    _mediator.Send(new DeleteYerbaMateCommand { YerbaMateId = Guid.Empty })).Should()
                .Throw<NotFoundException>();
        }

        /// <summary>
        ///     Delete yerba mate command should delete yerba mate
        /// </summary>
        [Fact]
        public async Task ShouldDeleteYerbaMate()
        {
            await TestSeeder.SeedTestYerbaMatesAsync(_factory);

            var yerbaMateId = Guid.Parse("3C24EB64-6CA5-4716-9A9A-42654F0EAF43"); //one of seeded yerba mates

            //delete
            await _mediator.Send(new DeleteYerbaMateCommand { YerbaMateId = yerbaMateId });

            //Assert that deleted
            var item = await DbHelper.FindAsync<YerbaMate>(_factory, yerbaMateId);
            item.Should().BeNull();
        }

        //Delete cascade is probably not supported in InMemoryDb

        // /// <summary>
        // /// Delete yerba mate should delete all opinions about this yerba mate
        // /// </summary>
        // [Fact]
        // public async Task DeleteYerbaMateShouldDeleteAllOpinionsAboutThisYerbaMate()
        // {
        //     await TestSeeder.SeedTestYerbaMatesAsync(_factory);
        //     await TestSeeder.SeedTestOpinionsAsync(_factory);
        //
        //     //one of seeded yerba mates
        //     var yerbaMateId = Guid.Parse("3C24EB64-6CA5-4716-9A9A-42654F0EAF43");
        //
        //     //seeded opinions about this yerba mate
        //     var yerbaMateOpinions = new List<Guid>()
        //     {
        //         Guid.Parse("EB2BB300-A4FF-486C-AB64-4EF0A7DB527F"),
        //         Guid.Parse("E3544051-5179-4181-B1D9-662DF4BE7797")
        //     };
        //
        //     //delete
        //     await _mediator.Send(new DeleteYerbaMateCommand { YerbaMateId = yerbaMateId });
        //
        //     //Assert that opinions about this yerba mate deleted
        //
        //     foreach (var opinionId in yerbaMateOpinions)
        //     {
        //         var item = await DbHelper.FindAsync<Opinion>(_factory, opinionId);
        //         item.Should().BeNull();
        //     }
        // }

        //Delete cascade is probably not supported in InMemoryDb

        // /// <summary>
        // /// Delete yerba mate should delete all favourites with this yerba mate
        // /// </summary>
        // [Fact]
        // public async Task DeleteYerbaMateShouldDeleteAllFavouritesWithThisYerbaMate()
        // {
        //     await TestSeeder.SeedTestYerbaMatesAsync(_factory);
        //     await TestSeeder.SeedTestFavouritesAsync(_factory);
        //
        //     //one of seeded yerba mates
        //     var yerbaMateId = Guid.Parse("3C24EB64-6CA5-4716-9A9A-42654F0EAF43");
        //
        //     //seeded favourites about this yerba mate
        //     var yerbaMateFavourites = new List<Guid>()
        //     {
        //         Guid.Parse("5F07ACEC-B726-4DA8-968D-5088543D6D85"),
        //         Guid.Parse("042C8BB9-311C-49B6-84D1-1E4993038270")
        //     };
        //
        //     //delete
        //     await _mediator.Send(new DeleteYerbaMateCommand { YerbaMateId = yerbaMateId });
        //
        //     //Assert that favourites with this yerba mate deleted
        //     foreach (var favouriteId in yerbaMateFavourites)
        //     {
        //         var item = await DbHelper.FindAsync<Favourite>(_factory, favouriteId);
        //         item.Should().BeNull();
        //     }
        // }
    }
}