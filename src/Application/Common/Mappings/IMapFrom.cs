using AutoMapper;

namespace Application.Common.Mappings;

/// <summary>
///     IMapFrom interface
/// </summary>
/// <typeparam name="T">Source type</typeparam>
public interface IMapFrom<T>
{
    /// <summary>
    ///     Creates map
    /// </summary>
    /// <param name="profile">The profile</param>
    void Mapping(Profile profile)
    {
        profile.CreateMap(typeof(T), GetType());
    }
}