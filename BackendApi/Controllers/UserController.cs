using BackendApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BackendApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly StoreDBContext context;

        public UserController(UserManager<User> userManager,
                              SignInManager<User> signInManager,
                              StoreDBContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
        }

        [HttpPost, Route("login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            if (user == null)
                return BadRequest("invalid client request");

            var result = await signInManager.PasswordSignInAsync(user.UserName, user.PasswordHash, isPersistent: false, lockoutOnFailure: false);
            if(result.Succeeded)
            {
                User.IsInRole("LoggedIn");
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("abdou@nawar12345"));
                var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var tokenOptions = new JwtSecurityToken(
                        issuer: "https://localhost:64127",
                        audience: "https://localhost:64127",
                        claims: new List<Claim>(),
                        expires: DateTime.Now.AddMinutes(5),
                        signingCredentials: signingCredentials
                    );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                var userId = context.Users.FirstOrDefault(u => u.UserName == user.UserName).Id;
                return Ok(new { Token = tokenString, userId = userId });
            }

            return Unauthorized();
        }

        [HttpPost, Route("register")]
        public async Task<IActionResult> register([FromBody] User user)
        {
            var addedUser = new User()
            {
                Email = user.Email,
                UserName = user.UserName,
                PasswordHash = user.PasswordHash
            };

            var result = await userManager.CreateAsync(addedUser, user.PasswordHash);
            if (result.Succeeded)
            {
                await signInManager.SignInAsync(addedUser, isPersistent: false);
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("abdou@nawar12345"));
                var signingCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var tokenOptions = new JwtSecurityToken(
                        issuer: "https://localhost:64127",
                        audience: "https://localhost:64127",
                        claims: new List<Claim>(),
                        expires: DateTime.Now.AddMinutes(5),
                        signingCredentials: signingCredentials
                    );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                var userId = context.Users.FirstOrDefault(u => u.UserName == user.UserName).Id;
                return Ok(new { Token = tokenString, userId = userId });
            }
            else
            {
                return Unauthorized(result.Errors);
            }
        }

    }
}
