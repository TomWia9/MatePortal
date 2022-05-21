using System;
using System.Threading.Tasks;
using Application.Categories.Commands.DeleteCategory;
using Application.Common.Exceptions;
using Application.IntegrationTests.Helpers;
using Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.Categories.Commands;

/// <summary>
///     Delete category tests
/// </summary>
public class DeleteCategoryTests : IntegrationTest
{
    /// <summary>
    ///     Delete category with incorrect id should throw not found exception
    /// </summary>
    [Fact]
    public void DeleteCategoryWithIncorrectIdShouldThrowNotFound()
    {
        var categoryId = Guid.Empty;

        FluentActions.Invoking(() =>
                _mediator.Send(new DeleteCategoryCommand {CategoryId = categoryId})).Should()
            .ThrowAsync<NotFoundException>();
    }

    /// <summary>
    ///     Delete category command should delete category
    /// </summary>
    [Fact]
    public async Task ShouldDeleteCategory()
    {
        await TestSeeder.SeedTestCategoriesAsync(_factory);

        var categoryId = Guid.Parse("8438FB5B-DC77-40F2-ABB6-C7DCE326571E"); //one of seeded category

        //delete
        await _mediator.Send(new DeleteCategoryCommand {CategoryId = categoryId});

        //Assert that deleted
        var item = await DbHelper.FindAsync<Category>(_factory, categoryId);
        item.Should().BeNull();
    }
}