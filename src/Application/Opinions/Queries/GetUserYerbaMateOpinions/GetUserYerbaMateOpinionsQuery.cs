using Application.Common.Models;
using MediatR;

namespace Application.Opinions.Queries.GetUserYerbaMateOpinions
{
    /// <summary>
    /// Get all opinions about yerba mate posted by single user
    /// </summary>
    public class GetUserYerbaMateOpinionsQuery : IRequest<PaginatedList<OpinionDto>>
    {
        /// <summary>
        /// Initializes GetUserYerbaMateOpinionsQuery
        /// </summary>
        /// <param name="parameters">Opinions query parameters</param>
        public GetUserYerbaMateOpinionsQuery(OpinionsQueryParameters parameters)
        {
            Parameters = parameters;
        }

        /// <summary>
        /// Opinions query parameters
        /// </summary>
        private OpinionsQueryParameters Parameters { get; }
    }
}