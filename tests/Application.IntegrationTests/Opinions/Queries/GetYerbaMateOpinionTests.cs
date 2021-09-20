﻿using System;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.IntegrationTests.Helpers;
using Application.Opinions.Queries;
using Application.Opinions.Queries.GetYerbaMateOpinion;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.Opinions.Queries
{
    /// <summary>
    ///     Get single yerba mate opinion tests
    /// </summary>
    public class GetYerbaMateOpinionTests : IntegrationTest
    {
        /// <summary>
        ///     Get yerba mate opinion command should return correct opinion data transfer object
        /// </summary>
        [Fact]
        public async Task ShouldReturnCorrectOpinion()
        {
            await TestSeeder.SeedTestOpinionsAsync(_factory);

            var opinionId = Guid.Parse("EB2BB300-A4FF-486C-AB64-4EF0A7DB527F"); //id of one of seeded opinion

            var expectedResult = new OpinionDto
            {
                Id = opinionId,
                Rate = 10,
                Comment = "Comment 1"
            };

            var response = await _mediator.Send(new GetYerbaMateOpinionQuery(opinionId));

            response.Should().BeOfType<OpinionDto>();
            response.Id.Should().Be(opinionId);
            response.Rate.Should().Be(expectedResult.Rate);
            response.Comment.Should().Be(expectedResult.Comment);
        }

        /// <summary>
        ///     Get yerba mate opinion with incorrect id should throw not found exception
        /// </summary>
        [Fact]
        public void GetYerbaMateOpinionWithIncorrectIdShouldThrowNotFound()
        {
            var yerbaMateOpinionId = Guid.Empty;

            FluentActions.Invoking(() =>
                _mediator.Send(new GetYerbaMateOpinionQuery(yerbaMateOpinionId))).Should().Throw<NotFoundException>();
        }
    }
}