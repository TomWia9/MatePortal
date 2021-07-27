using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;

namespace Infrastructure.Services
{
    /// <summary>
    /// Yerba mate service
    /// </summary>
    public class YerbaMateService : IYerbaMateService
    {
        /// <summary>
        /// Database context
        /// </summary>
        private readonly IApplicationDbContext _context;

        /// <summary>
        /// Initializes YerbaMateService
        /// </summary>
        /// <param name="context">Database context</param>
        public YerbaMateService(IApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Increases number of additions yerba mate to favorites
        /// </summary>
        /// <param name="yerbaMateId">Yerba mate ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public async Task IncreaseNumberOfAddToFav(Guid yerbaMateId, CancellationToken cancellationToken)
        {
            var yerbaMate = await _context.YerbaMate.FindAsync(yerbaMateId);

            yerbaMate.NumberOfAddToFav += 1;

            await _context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Decreases number of additions yerba mate to favorites
        /// </summary>
        /// <param name="yerbaMateId">Yerba mate ID</param>
        /// <param name="cancellationToken">Cancellation token</param>
        public async Task DecreaseNumberOfAddToFav(Guid yerbaMateId, CancellationToken cancellationToken)
        {
            var yerbaMate = await _context.YerbaMate.FindAsync(yerbaMateId);

            yerbaMate.NumberOfAddToFav -= 1;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}