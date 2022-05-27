using System;
using System.Runtime.Serialization;
using Application.Brands.Queries;
using Application.Categories.Queries;
using Application.Common.Mappings;
using Application.Countries.Queries;
using Application.Favourites.Queries;
using Application.ShopOpinions.Queries;
using Application.Shops.Queries;
using Application.YerbaMateOpinions.Queries;
using Application.YerbaMates.Queries;
using AutoMapper;
using Domain.Entities;
using Xunit;

namespace Application.UnitTests.Common.Mappings;

/// <summary>
///     Mapping tests
/// </summary>
public class MappingTests
{
    /// <summary>
    ///     The configuration
    /// </summary>
    private readonly IConfigurationProvider _configuration;

    /// <summary>
    ///     The mapper
    /// </summary>
    private readonly IMapper _mapper;

    /// <summary>
    ///     Initializes the MappingTests
    /// </summary>
    public MappingTests()
    {
        _configuration = new MapperConfiguration(cfg => { cfg.AddProfile<MappingProfile>(); });

        _mapper = _configuration.CreateMapper();
    }

    /// <summary>
    ///     Mappings should have valid configuration
    /// </summary>
    [Fact]
    public void ShouldHaveValidConfiguration()
    {
        _configuration.AssertConfigurationIsValid();
    }

    /// <summary>
    ///     Mappings should support mapping from source to destination
    /// </summary>
    /// <param name="source">The source type</param>
    /// <param name="destination">The destination type</param>
    [Theory]
    [InlineData(typeof(Brand), typeof(BrandDto))]
    [InlineData(typeof(Category), typeof(CategoryDto))]
    [InlineData(typeof(Country), typeof(CountryDto))]
    [InlineData(typeof(Favourite), typeof(FavouriteDto))]
    [InlineData(typeof(YerbaMateOpinion), typeof(YerbaMateOpinionDto))]
    [InlineData(typeof(ShopOpinion), typeof(ShopOpinionDto))]
    [InlineData(typeof(Shop), typeof(ShopDto))]
    [InlineData(typeof(YerbaMate), typeof(YerbaMateDto))]
    public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
    {
        var instance = GetInstanceOf(source);
        _mapper.Map(instance, source, destination);
    }

    /// <summary>
    ///     Gets instance of given type
    /// </summary>
    /// <param name="type">The type</param>
    /// <returns>The instance of the specified type</returns>
    private static object GetInstanceOf(Type type)
    {
        if (type.GetConstructor(Type.EmptyTypes) != null)
            return Activator.CreateInstance(type);

        // Type without parameterless constructor
        return FormatterServices.GetUninitializedObject(type);
    }
}