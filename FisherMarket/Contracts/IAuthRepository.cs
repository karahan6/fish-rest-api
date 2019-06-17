using FisherMarket.Models;
using System.Threading.Tasks;

namespace FisherMarket.Contracts
{
    public interface IAuthRepository
    {
        Task<User> Register(User user, string password);
        Task<User> Login(string username, string password);
        Task<bool> UserExists(string username);
    }
}
