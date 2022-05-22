using System;
using System.Threading.Tasks;
using Application.Categories.Commands.CreateCategory;
using Application.Categories.Queries;
using Application.Common.Exceptions;
using Application.IntegrationTests.Helpers;
using Domain.Entities;
using FluentAssertions;
using FluentAssertions.Extensions;
using Xunit;

namespace Application.IntegrationTests.Categories.Commands;

/// <summary>
///     Create category tests
/// </summary>
public class CreateCategoryTests : IntegrationTest
{
    /// <summary>
    ///     Create category should create category and return category data transfer object
    /// </summary>
    [Fact]
    public async Task ShouldCreateCategoryAndReturnCategoryDto()
    {
        var userId = await AuthHelper.RunAsAdministratorAsync(Factory);

        var command = new CreateCategoryCommand
        {
            Name = "Test category",
            Description = "Test description"
        };

        var expectedResult = new CategoryDto
        {
            Name = command.Name,
            Description = command.Description
        };

        var result = await Mediator.Send(command);

        result.Should().BeOfType<CategoryDto>();
        result.Should().BeEquivalentTo(expectedResult, x => x.Excluding(y => y.Id));

        var item = await DbHelper.FindAsync<Category>(Factory, result.Id);

        item.CreatedBy.Should().NotBeNull();
        item.CreatedBy.Should().Be(userId);
        item.Created.Should().BeCloseTo(DateTime.Now, 1.Seconds());
        item.LastModified.Should().BeNull();
        item.LastModifiedBy.Should().BeNull();
    }

    /// <summary>
    ///     Category should require unique name
    /// </summary>
    [Fact]
    public async Task ShouldRequireUniqueName()
    {
        await Mediator.Send(new CreateCategoryCommand
        {
            Name = "Test",
            Description = "Test"
        });

        var command = new CreateCategoryCommand
        {
            Name = "Test",
            Description = "Test"
        };

        await FluentActions.Invoking(() =>
            Mediator.Send(command)).Should().ThrowAsync<ConflictException>();
    }
}