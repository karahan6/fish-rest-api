using FisherMarket.Contracts;
using FisherMarket.Data;
using FisherMarket.Models;

namespace FisherMarket.Repository
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(DataContext context) : base(context)
        {
        }
    }
}
