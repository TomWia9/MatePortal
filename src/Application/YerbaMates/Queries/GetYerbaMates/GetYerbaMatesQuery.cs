using Application.Common.Models;
using MediatR;

namespace Application.YerbaMates.Queries.GetYerbaMates
{
    /// <summary>
    /// Get all yerba mates query
    /// </summary>
    public class GetYerbaMatesQuery : IRequest<PaginatedList<YerbaMateDto>>
    {
        /// <summary>
        /// Initializes GetYerbaMatesQuery
        /// </summary>
        /// <param name="parameters">Yerba mates query parameters</param>
        public GetYerbaMatesQuery(YerbaMatesQueryParameters parameters)
        {
            Parameters = parameters;
        }

        /// <summary>
        /// Yerba mates query parameters
        /// </summary>
        private YerbaMatesQueryParameters Parameters { get; }
    }
}