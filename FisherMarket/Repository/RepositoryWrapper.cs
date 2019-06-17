using FisherMarket.Contracts;
using FisherMarket.Data;

namespace FisherMarket.Repository
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private DataContext _context;
        private IUserRepository _user;
        private IFishRepository _fish;
        private IRoleRepository _role;

        public RepositoryWrapper(DataContext context)
        {
            _context = context;
        }
        public IUserRepository User
        {
            get
            {
                if (_user == null)
                {
                    _user = new UserRepository(_context);
                }

                return _user;
            }
        }

        public IFishRepository Fish
        {
            get
            {
                if (_fish == null)
                {
                    _fish = new FishRepository(_context);
                }

                return _fish;
            }
        }


        public IRoleRepository Role
        {
            get
            {
                if (_role == null)
                {
                    _role = new RoleRepository(_context);
                }

                return _role;
            }
        }


        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
