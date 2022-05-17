using System;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.IntegrationTests.Helpers;
using Application.Opinions.Commands.CreateOpinion;
using Application.Opinions.Queries;
using Application.YerbaMates.Queries.GetYerbaMate;
using Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.Opinions.Commands
{
    /// <summary>
    /// Create yerba mate opinion tests
    /// </summary>
    public class CreateOpinionTests : IntegrationTest
    {
        /// <summary>
        /// Create yerba mate opinion should create opinion and return yerba mate opinion data transfer object
        /// </summary>
        [Fact]
        public async Task ShouldCreateYerbaMateOpinionAndReturnYerbaMateOpinionDto()
        {
            await TestSeeder.SeedTestYerbaMatesAsync(_factory);

            var userId = await AuthHelper.RunAsDefaultUserAsync(_factory);

            var command = new CreateOpinionCommand
            {
                Rate = 8,
                Comment = "Test comment",
                YerbaMateId = Guid.Parse("3C24EB64-6CA5-4716-9A9A-42654F0EAF43") //one of seeded yerba mate
            };

            var result = await _mediator.Send(command);

            var item = await DbHelper.FindAsync<Opinion>(_factory, result.Id);

            result.Should().BeOfType<OpinionDto>();
            result.Rate.Should().Be(command.Rate);
            result.Comment.Should().Be(command.Comment);
            result.Created.Should().BeCloseTo(DateTime.Now, 1000);
            result.YerbaMateId.Should().Be(command.YerbaMateId);
            result.CreatedBy.Should().Be(userId);

            item.CreatedBy.Should().NotBeNull();
            item.CreatedBy.Should().Be(userId);
            item.Created.Should().BeCloseTo(DateTime.Now, 1000);
            item.LastModified.Should().BeNull();
            item.LastModifiedBy.Should().BeNull();
        }


        /// <summary>
        /// Create opinion for nonexistent yerba should throw NotFound
        /// </summary>
        [Fact]
        public async Task CreateOpinionForNonexistentYerbaMateShouldThrowNotFound()
        {
            await AuthHelper.RunAsDefaultUserAsync(_factory);
            var yerbaMateId = Guid.NewGuid();

            var command = new CreateOpinionCommand
            {
                Rate = 2,
                Comment = "test",
                YerbaMateId = yerbaMateId
            };

            FluentActions.Invoking(() =>
                _mediator.Send(command)).Should().Throw<NotFoundException>();
        }

        /// <summary>
        /// Opinion should not be added more than once to one yerba by one user
        /// </summary>
        [Fact]
        public async Task OpinionShouldNotBeAddedMoreThanOnceToOneYerbaByOneUser()
        {
            await TestSeeder.SeedTestYerbaMatesAsync(_factory);
            await AuthHelper.RunAsDefaultUserAsync(_factory);
            var yerbaMateId = Guid.Parse("3C24EB64-6CA5-4716-9A9A-42654F0EAF43"); //id of one of seeded yerba mate

            await _mediator.Send(new CreateOpinionCommand
            {
                Rate = 10,
                Comment = "Test",
                YerbaMateId = yerbaMateId
            });

            var command = new CreateOpinionCommand
            {
                Rate = 8,
                Comment = "Test 2",
                YerbaMateId = yerbaMateId
            };

            FluentActions.Invoking(() =>
                _mediator.Send(command)).Should().Throw<ConflictException>();
        }

        /// <summary>
        /// Create should increase yerba mate number of opinions
        /// </summary>
        [Fact]
        public async Task ShouldIncreaseYerbaMateNumberOfOpinions()
        {
            await TestSeeder.SeedTestBrandsAsync(_factory);
            await TestSeeder.SeedTestCategoriesAsync(_factory);
            await TestSeeder.SeedTestYerbaMatesAsync(_factory);

            await AuthHelper.RunAsDefaultUserAsync(_factory);

            var command = new CreateOpinionCommand
            {
                Comment = "Test",
                Rate = 8,
                YerbaMateId = Guid.Parse("3C24EB64-6CA5-4716-9A9A-42654F0EAF43") //one of seeded yerba mate
            };

            await _mediator.Send(command);


            var yerbaMateDto = await _mediator.Send(new GetYerbaMateQuery(command.YerbaMateId));
            yerbaMateDto.NumberOfOpinions.Should().Be(1);
        }
    }
}