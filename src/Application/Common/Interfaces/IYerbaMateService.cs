using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Common.Interfaces
{
    /// <summary>
    /// Yerba mate service interface
    /// </summary>
    public interface IYerbaMateService
    {
        /// <summary>
        /// Increases number of additions yerba mate to favorites
        /// </summary>
        /// <param name="yerbaMateId">Yerba mate ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task IncreaseNumberOfAddToFav(Guid yerbaMateId, CancellationToken cancellationToken);
        
        /// <summary>
        /// Decreases number of additions yerba mate to favorites
        /// </summary>
        /// <param name="yerbaMateId">Yerba mate ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        Task DecreaseNumberOfAddToFav(Guid yerbaMateId, CancellationToken cancellationToken);
    }
}