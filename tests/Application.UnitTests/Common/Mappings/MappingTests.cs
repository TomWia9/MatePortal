using System;
using System.Runtime.Serialization;
using Application.Brands.Queries.GetBrands;
using Application.Categories.Queries.GetCategories;
using Application.Common.Mappings;
using Application.Countries.Queries.GetCountries;
using Application.Favourites.Queries.GetFavourites;
using Application.Opinions.Queries.GetOpinions;
using Application.ShopOpinions.Queries.GetShopOpinions;
using Application.Shops.Queries.GetShops;
using Application.Users.Queries.GetUser;
using Application.YerbaMates.Queries.GetYerbaMate;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Identity;
using Xunit;

namespace Application.UnitTests.Common.Mappings
{
    public class MappingTests
    {
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;

        public MappingTests()
        {
            _configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = _configuration.CreateMapper();
        }

        [Fact]
        public void ShouldHaveValidConfiguration()
        {
            _configuration.AssertConfigurationIsValid();
        }

        [Theory]
        [InlineData(typeof(Brand), typeof(BrandDto))]
        [InlineData(typeof(Category), typeof(CategoryDto))]
        [InlineData(typeof(Country), typeof(CountryDto))]
        [InlineData(typeof(Favourite), typeof(FavouriteDto))]
        [InlineData(typeof(Opinion), typeof(OpinionDto))]
        [InlineData(typeof(ShopOpinion), typeof(ShopOpinionDto))]
        [InlineData(typeof(Shop), typeof(ShopDto))]
        [InlineData(typeof(YerbaMate), typeof(YerbaMateDto))]
        //[InlineData(typeof(ApplicationUser), typeof(UserDto))]
        public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
        {
            var instance = GetInstanceOf(source);
            _mapper.Map(instance, source, destination);
        }

        private static object GetInstanceOf(Type type)
        {
            if (type.GetConstructor(Type.EmptyTypes) != null)
                return Activator.CreateInstance(type);

            // Type without parameterless constructor
            return FormatterServices.GetUninitializedObject(type);
        }
    }
}