using System;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.IntegrationTests.Helpers;
using Application.Opinions.Commands.CreateOpinion;
using Application.Opinions.Commands.UpdateOpinion;
using Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.Opinions.Commands
{
    /// <summary>
    /// Update opinion tests
    /// </summary>
    public class UpdateOpinionTests : IntegrationTest
    {
        /// <summary>
        /// Update opinion with incorrect id should throw not found exception
        /// </summary>
        [Fact]
        public void UpdateOpinionWithIncorrectIdShouldThrowNotFound()
        {
            var opinionId = Guid.Empty;

            var command = new UpdateOpinionCommand
            {
                OpinionId = opinionId,
                Rate = 2,
                Comment = "Updated comment"
            };

            FluentActions.Invoking(() =>
                _mediator.Send(command)).Should().Throw<NotFoundException>();
        }

        /// <summary>
        /// Update opinion command should update opinion
        /// </summary>
        [Fact]
        public async Task UpdateOpinionShouldUpdateOpinion()
        {
            var userId = await AuthHelper.RunAsDefaultUserAsync(_factory);
            await TestSeeder.SeedTestOpinionsAsync(_factory);

            var opinionId = Guid.Parse("EB2BB300-A4FF-486C-AB64-4EF0A7DB527F"); //one of seeded opinions

            var command = new UpdateOpinionCommand
            {
                OpinionId = opinionId,
                Rate = 4,
                Comment = "Updated comment"
            };

            await _mediator.Send(command);

            var item = await DbHelper.FindAsync<Opinion>(_factory, opinionId);

            item.Rate.Should().Be(command.Rate);
            item.Comment.Should().Be(command.Comment);
            item.CreatedBy.Should().NotBeNull();
            item.LastModified.Should().NotBeNull();
            item.LastModified.Should().BeCloseTo(DateTime.Now, 1000);
            item.LastModifiedBy.Should().NotBeNull();
            item.LastModifiedBy.Should().Be(userId);
        }

        /// <summary>
        /// User should not be able to update other user opinion
        /// </summary>
        [Fact]
        public async Task UserShouldNotBeAbleToUpdateOtherUserOpinion()
        {
            await TestSeeder.SeedTestYerbaMatesAsync(_factory);
            await AuthHelper.RunAsDefaultUserAsync(_factory);

            //create opinion firstly
            var opinionToUpdateDto = await _mediator.Send(new CreateOpinionCommand
            {
                Rate = 10,
                Comment = "test",
                YerbaMateId = Guid.Parse("3C24EB64-6CA5-4716-9A9A-42654F0EAF43") //id of one of seeded yerba mate
            });

            _factory.CurrentUserId = Guid.NewGuid(); //other user

            FluentActions.Invoking(() =>
                    _mediator.Send(new UpdateOpinionCommand
                        { OpinionId = opinionToUpdateDto.Id, Comment = "test", Rate = 1 })).Should()
                .Throw<ForbiddenAccessException>();
        }

        /// <summary>
        /// Administrator should be able to update user opinion
        /// </summary>
        [Fact]
        public async Task AdministratorShouldBeAbleToUpdateUserOpinion()
        {
            await AuthHelper.RunAsDefaultUserAsync(_factory);
            await TestSeeder.SeedTestYerbaMatesAsync(_factory);

            //create opinion firstly
            var opinionToUpdateDto = await _mediator.Send(new CreateOpinionCommand
            {
                Rate = 10,
                Comment = "test",
                YerbaMateId = Guid.Parse("3C24EB64-6CA5-4716-9A9A-42654F0EAF43") //id of one of seeded yerba mate
            });

            await AuthHelper.RunAsAdministratorAsync(_factory);

            var command = new UpdateOpinionCommand
            {
                OpinionId = opinionToUpdateDto.Id,
                Comment = "test1",
                Rate = 2
            };

            await _mediator.Send(command);

            //Assert that updated
            var item = await DbHelper.FindAsync<Opinion>(_factory, opinionToUpdateDto.Id);
            item.Rate.Should().Be(command.Rate);
            item.Comment.Should().Be(command.Comment);
            item.CreatedBy.Should().NotBeNull();
            item.LastModified.Should().BeCloseTo(DateTime.Now, 1000);
            item.LastModifiedBy.Should().NotBeNull();
        }
    }
}