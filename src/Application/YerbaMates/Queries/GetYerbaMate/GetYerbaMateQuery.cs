using System;
using MediatR;

namespace Application.YerbaMates.Queries.GetYerbaMate
{
    /// <summary>
    /// Get single yerba mate query
    /// </summary>
    public class GetYerbaMateQuery : IRequest<YerbaMateDto>
    {
        /// <summary>
        /// Initializes GetYerbaMateQuery
        /// </summary>
        /// <param name="yerbaMateId">Yerba mate ID</param>
        public GetYerbaMateQuery(Guid yerbaMateId)
        {
            YerbaMateId = yerbaMateId;
        }

        /// <summary>
        /// Yerba mate ID
        /// </summary>
        private Guid YerbaMateId { get; }
    }
}