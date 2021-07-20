using System;
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
        /// <param name="userId">User ID</param>
        /// <param name="parameters">Opinions query parameters</param>
        public GetUserYerbaMateOpinionsQuery(Guid userId, OpinionsQueryParameters parameters)
        {
            UserId = userId;
            Parameters = parameters;
        }

        /// <summary>
        /// User ID
        /// </summary>
        private Guid UserId { get; }

        /// <summary>
        /// Opinions query parameters
        /// </summary>
        private OpinionsQueryParameters Parameters { get; }
    }
}