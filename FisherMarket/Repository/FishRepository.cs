using FisherMarket.Contracts;
using FisherMarket.Data;
using FisherMarket.Models;

namespace FisherMarket.Repository
{
    public class FishRepository : RepositoryBase<Fish>, IFishRepository
    {
        public FishRepository(DataContext context) : base(context)
        {
        }
    }
}
