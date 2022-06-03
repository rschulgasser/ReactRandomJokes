using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RandomJokes.Data;
using RandomJokes.Web.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RandomJokes.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

        public class Account : ControllerBase
        {
            private string _connectionString;
            private IConfiguration _configuration;

            public Account(IConfiguration configuration)
            {
                _connectionString = configuration.GetConnectionString("ConStr");
                _configuration = configuration;
            }

            [HttpPost]
            [Route("signup")]
            public void Signup(SignupViewModel user)
            {
                var repo = new UserRepository(_connectionString);
                repo.AddUser(user, user.Password);
            }

            [HttpPost]
            [Route("login")]
            public IActionResult Login(LoginViewModel viewModel)
            {
                var repo = new UserRepository(_connectionString);
                var user = repo.Login(viewModel.Email, viewModel.Password);
                if (user == null)
                {
                    return Unauthorized();
                }

                var claims = new List<Claim>()
            {
                new Claim("user", viewModel.Email)
            };

                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JWTSecret")));
                var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokenOptions = new JwtSecurityToken(signingCredentials: credentials,
                    claims: claims);
                string tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                return Ok(new { token = tokenString });
            }

            [HttpGet]
            [Route("getcurrentuser")]
            public User GetCurrentUser()
            {
                string email = User.FindFirst("user")?.Value;
                if (String.IsNullOrEmpty(email))
                {
                    return null;
                }

                var repo = new UserRepository(_connectionString);
                return repo.GetByEmail(email);
            }
        }
    
}
