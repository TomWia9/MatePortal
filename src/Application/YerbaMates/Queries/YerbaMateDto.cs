using System;
using Application.Brands.Queries;
using Application.Categories.Queries;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.YerbaMates.Queries;

/// <summary>
///     Yerba mate data transfer object
/// </summary>
public class YerbaMateDto : IMapFrom<YerbaMate>
{
    /// <summary>
    ///     Yerba mate ID
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    ///     Yerba mate name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    ///     Yerba mate description
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    ///     Yerba mate image url
    /// </summary>
    public string ImgUrl { get; set; }

    /// <summary>
    ///     Yerba ate average price
    /// </summary>
    public decimal AveragePrice { get; set; }

    /// <summary>
    ///     Yerba mate number of add to fav
    /// </summary>
    public int NumberOfAddToFav { get; set; }

    /// <summary>
    ///     Yerba mate number of opinions
    /// </summary>
    public int NumberOfOpinions { get; set; }

    /// <summary>
    ///     Yerba mate brand
    /// </summary>
    public BrandDto Brand { get; set; }

    /// <summary>
    ///     Yerba mate category
    /// </summary>
    public CategoryDto Category { get; set; }

    /// <summary>
    ///     Overrides Mapping method from IMapFrom interface by adding a custom NumberOfAddToFav mapping
    /// </summary>
    /// <param name="profile">Automapper profile</param>
    public void Mapping(Profile profile)
    {
        profile.CreateMap<YerbaMate, YerbaMateDto>()
            .ForMember(d => d.NumberOfAddToFav, opt => opt.MapFrom(y => y.Favourites.Count))
            .ForMember(d => d.NumberOfOpinions, opt => opt.MapFrom(y => y.YerbaMateOpinions.Count));
    }
}