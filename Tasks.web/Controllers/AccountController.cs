using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tasks.data;
using Tasks.web.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace Tasks.web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly string _connectionString;
        private IConfiguration _configuration;

        public AccountController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
            _configuration = configuration;
        }
        [HttpPost]
        [Route("Signup")]
        public void Signup(SignupViewModel viewModel)
        {
            var repo = new UserRepository(_connectionString);
            repo.AddUser(viewModel, viewModel.Password);
        }
        [HttpGet]
        [Route("GetCurrentUser")]
        public User GetCurrentUser()
        {
            string userId = User.FindFirst("user")?.Value;
            if (String.IsNullOrEmpty(userId))
            {
                 return null;
            }
            var repo = new UserRepository(_connectionString);
            return repo.GetUserByEmail(userId);

        }
        [HttpPost]
        [Route("Login")]
      public IActionResult Login(LoginViewModel viewModel)
        {
            var repo = new UserRepository(_connectionString);
            var user = repo.Login(viewModel.Email, viewModel.Password);
            if (user == null)
            {
                return Unauthorized();
            }
            var claims = new List<Claim>
            {
                new Claim("user", viewModel.Email)
            };

            SymmetricSecurityKey secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JWTSecret")));
            var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokenOptions = new JwtSecurityToken(signingCredentials: credentials,
                claims: claims);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return Ok(new { token = tokenString });
        }


    }

}
