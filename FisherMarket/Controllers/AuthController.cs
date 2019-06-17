using AutoMapper;
using FisherMarket.Contracts;
using FisherMarket.DTOs;
using FisherMarket.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FisherMarket.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly IRepositoryWrapper _repo;
        private readonly ILoggerManager _logger;

        public AuthController(IConfiguration config, 
            UserManager<User> userManager,
            IMapper mapper,
            SignInManager<User> signInManager,
            IEmailSender emailSender,
            IRepositoryWrapper repo,
            ILoggerManager logger)
        {
            _config = config;
            _userManager = userManager;
            _mapper = mapper;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _repo = repo;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserDto userDto)
        {
            _logger.LogInfo("POST api/auth/login");

            var user = await _userManager.FindByNameAsync(userDto.Username);
            if(await _userManager.CheckPasswordAsync(user, userDto.Password))
            {
                if (await _userManager.IsEmailConfirmedAsync(user))
                {
                    var result = await _signInManager.CheckPasswordSignInAsync(user, userDto.Password, false);
                    if (result.Succeeded)
                    {
                        var appUser = await _userManager.Users.FirstOrDefaultAsync(u => u.UserName == userDto.Username);

                        return Ok(new
                        {
                            token = GenerateJwtToken(appUser).Result
                        });
                    }

                    return Unauthorized();
                }
                else
                {
                    return StatusCode(Microsoft.AspNetCore.Http.StatusCodes.Status412PreconditionFailed,
                        new
                        {
                            message = "Email adresiniz doğrulanmamıştır!",
                            userId = user.Id
                        });

                }

            }

            return Unauthorized();
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            _logger.LogInfo("POST api/auth/register");

            var userToCreate = _mapper.Map<User>(userDto);
            userToCreate.Email = userToCreate.UserName;
            var result = await _userManager.CreateAsync(userToCreate, userDto.Password);
            var emailToken = await _userManager.GenerateEmailConfirmationTokenAsync(userToCreate);
            int randomToken = new Random().Next(10000, 99999);
            userToCreate.EmailConfirmToken = emailToken;
            userToCreate.RandomConfirm = randomToken;
            await _userManager.AddToRoleAsync(userToCreate, "Member");
            await _userManager.UpdateAsync(userToCreate);
            if (result.Succeeded)
            {
                await _emailSender.SendEmailAsync(userToCreate.Email, "Confirm your Fish-Market account",
                $"Please confirm your account by entering the number: " + randomToken );
                return Ok(new
                {
                    userId = userToCreate.Id
                });
            }

            return BadRequest(result.Errors);
        }

        [HttpGet("verify/{userId}/{randomToken}")]
        public async Task<ActionResult> VerifyEmailAsync(int randomToken, string userId)
        {
            _logger.LogInfo("GET api/auth/verify/{userId}/{randomToken} where userId = " + userId + " randomToken=" + randomToken);

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return Content("User not found");

            var confirmed = user.RandomConfirm.Equals(randomToken);
            if (confirmed)
            {
                var result = await _userManager.ConfirmEmailAsync(user, user.EmailConfirmToken);
                if (result.Succeeded)
                    return Ok(new
                    {
                        message = "İşlem Başarılı"
                    });
            }

            return BadRequest(new
            {
                message = "Hata alındı"
            });
        }

        private async Task<string> GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_config.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

    }
}
