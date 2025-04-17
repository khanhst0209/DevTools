using DevTools.Domain.Entities;
using DevTools.Infrastructure.Repositories.Interfaces;
using DevTools.Repositories;
using MyWebAPI.data;

namespace DevTools.Infrastructure.Repositories
{
    public class PremiumUpgradeRequestRepository : BaseRepository<PremiumUpgradeRequest, string>, IPremiumUpgradeRequestRepository
    {

        public PremiumUpgradeRequestRepository(MyDbContext context) : base(context)
        {
            
        }

    }
}