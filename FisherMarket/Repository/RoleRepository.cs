using FisherMarket.Contracts;
using FisherMarket.Data;
using FisherMarket.Models;

namespace FisherMarket.Repository
{
    public class RoleRepository : RepositoryBase<Fish>, IRoleRepository
    {
        public RoleRepository(DataContext context) : base(context)
        {
        }
    }
}
