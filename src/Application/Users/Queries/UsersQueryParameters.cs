using Application.Common.Extensions;
using Application.Common.QueryParameters;

namespace Application.Users.Queries;

/// <summary>
///     Users query parameters
/// </summary>
public class UsersQueryParameters : QueryParameters
{
    private readonly string _role;

    /// <summary>
    ///     Role parameter that indicates users roles
    /// </summary>
    public string Role
    {
        get => string.IsNullOrEmpty(_role) ? _role : _role.FirstCharToUpper();
        init => _role = value;
    }
}