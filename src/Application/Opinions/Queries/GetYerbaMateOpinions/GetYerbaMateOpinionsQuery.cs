using System;
using Application.Common.Models;
using MediatR;

namespace Application.Opinions.Queries.GetYerbaMateOpinions
{
    /// <summary>
    /// Get all opinions about single yerba mate query
    /// </summary>
    public class GetYerbaMateOpinionsQuery : IRequest<PaginatedList<OpinionDto>>
    {
        /// <summary>
        /// Initializes GetYerbaMateOpinionsQuery
        /// </summary>
        /// <param name="yerbaMateId">YerbaMate ID from which opinions can be obtained</param>
        /// <param name="parameters">Opinions query parameters</param>
        public GetYerbaMateOpinionsQuery(Guid yerbaMateId, OpinionsQueryParameters parameters)
        {
            YerbaMateId = yerbaMateId;
            Parameters = parameters;
        }

        /// <summary>
        /// YerbaMate ID from which opinions can be obtained
        /// </summary>
        private Guid YerbaMateId { get; }

        /// <summary>
        /// Opinions query parameters
        /// </summary>
        private OpinionsQueryParameters Parameters { get; }
    }
}