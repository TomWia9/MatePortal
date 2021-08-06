using System;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.IntegrationTests.Helpers;
using Application.Opinions.Commands.CreateOpinion;
using Application.Opinions.Commands.DeleteOpinion;
using Application.YerbaMates.Queries.GetYerbaMate;
using Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.Opinions.Commands
{
    /// <summary>
    /// Delete opinion tests
    /// </summary>
    public class DeleteOpinionTests : IntegrationTest
    {
        /// <summary>
        /// Delete opinion with incorrect id should throw not found exception
        /// </summary>
        [Fact]
        public void DeleteOpinionWithIncorrectIdShouldThrowNotFound()
        {
            var opinionId = Guid.Empty;

            FluentActions.Invoking(() =>
                    _mediator.Send(new DeleteOpinionCommand() {OpinionId = opinionId})).Should()
                .Throw<NotFoundException>();
        }

        /// <summary>
        /// Delete opinion command should delete opinion
        /// </summary>
        [Fact]
        public async Task ShouldDeleteFavourite()
        {
            await TestSeeder.SeedTestYerbaMatesAsync(_factory);
            await AuthHelper.RunAsDefaultUserAsync(_factory);

            //create opinion firstly
            var opinionToDeleteDto = await _mediator.Send(new CreateOpinionCommand()
            {
                Rate = 10,
                Comment = "test",
                YerbaMateId = Guid.Parse("3C24EB64-6CA5-4716-9A9A-42654F0EAF43") //id of one of seeded yerba mate
            });

            //delete
            await _mediator.Send(new DeleteOpinionCommand() {OpinionId = opinionToDeleteDto.Id});

            //Assert that deleted
            var item = await DbHelper.FindAsync<Opinion>(_factory, opinionToDeleteDto.Id);
            item.Should().BeNull();
        }

        /// <summary>
        /// User should not be able to delete other user opinion
        /// </summary>
        [Fact]
        public async Task UserShouldNotBeAbleToDeleteOtherUserOpinion()
        {
            await TestSeeder.SeedTestYerbaMatesAsync(_factory);
            var userId = await AuthHelper.RunAsDefaultUserAsync(_factory);

            //create opinion firstly
            var opinionToDeleteDto = await _mediator.Send(new CreateOpinionCommand()
            {
                Rate = 10,
                Comment = "test",
                YerbaMateId = Guid.Parse("3C24EB64-6CA5-4716-9A9A-42654F0EAF43") //id of one of seeded yerba mate
            });

            _factory.CurrentUserId = Guid.NewGuid(); //other user

            //delete
            FluentActions.Invoking(() =>
                    _mediator.Send(new DeleteOpinionCommand() {OpinionId = opinionToDeleteDto.Id})).Should()
                .Throw<ForbiddenAccessException>();
        }

        /// <summary>
        /// Administrator should be able to delete user opinion
        /// </summary>
        [Fact]
        public async Task AdministratorShouldBeAbleToDeleteUserOpinion()
        {
            await AuthHelper.RunAsDefaultUserAsync(_factory);
            await TestSeeder.SeedTestYerbaMatesAsync(_factory);

            //create opinion firstly
            var opinionToDeleteDto = await _mediator.Send(new CreateOpinionCommand()
            {
                Rate = 10,
                Comment = "test",
                YerbaMateId = Guid.Parse("3C24EB64-6CA5-4716-9A9A-42654F0EAF43") //id of one of seeded yerba mate
            });

            await AuthHelper
                .RunAsAdministratorAsync(_factory);
            
            //delete
            await _mediator.Send(new DeleteOpinionCommand() {OpinionId = opinionToDeleteDto.Id});

            //Assert that deleted
            var item = await DbHelper.FindAsync<Opinion>(_factory, opinionToDeleteDto.Id);
            item.Should().BeNull();
        }
        
        /// <summary>
        /// Delete should decrease yerba mate number of opinions
        /// </summary>
        [Fact]
        public async Task ShouldDecreaseYerbaMateNumberOfOpinions()
        {
            await TestSeeder.SeedTestBrandsAsync(_factory);
            await TestSeeder.SeedTestCategoriesAsync(_factory);
            await TestSeeder.SeedTestYerbaMatesAsync(_factory);

            await AuthHelper.RunAsDefaultUserAsync(_factory);

            var command = new CreateOpinionCommand()
            {
                Comment = "Test",
                Rate = 8,
                YerbaMateId = Guid.Parse("3C24EB64-6CA5-4716-9A9A-42654F0EAF43") //one of seeded yerba mate
            };

            var opinionToDeleteDto = await _mediator.Send(command);

            //delete
            await _mediator.Send(new DeleteOpinionCommand() { OpinionId = opinionToDeleteDto.Id });

            var yerbaMateDto = await _mediator.Send(new GetYerbaMateQuery(command.YerbaMateId));
            yerbaMateDto.NumberOfOpinions.Should().Be(0);
        }
    }
}