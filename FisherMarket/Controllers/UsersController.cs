using FisherMarket.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace FisherMarket.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IRepositoryWrapper _repo;
        private readonly ILoggerManager _logger;

        public UsersController(IRepositoryWrapper repo, ILoggerManager logger)
        {
            _logger = logger;
            _repo = repo;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            _logger.LogInfo("GET api/users");

            var users = _repo.User.FindAll();

            return Ok(users);
        }

        [HttpGet("{id}", Name = "GetUser")]
        public IActionResult GetUser(int id)
        {
            _logger.LogInfo("GET api/users/{id} where id=" + id);

            var user = _repo.User.FindByCondition(u => u.Id == id).FirstOrDefault();

            return Ok(user);
        }

    }
}
