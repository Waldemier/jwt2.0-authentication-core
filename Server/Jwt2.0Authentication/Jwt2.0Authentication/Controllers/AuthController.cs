using System.Linq;
using Jwt2._0Authentication.Data;
using Jwt2._0Authentication.Data.Dto;
using Jwt2._0Authentication.Data.Entities;
using Jwt2._0Authentication.Data.Enums;
using Jwt2._0Authentication.Security;
using Jwt2._0Authentication.Security.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Jwt2._0Authentication.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class AuthController: ControllerBase
    {
        private readonly IAppDbContext _context;
        private readonly IJwtService _jwtService;
        
        public AuthController(IAppDbContext context, IJwtService jwtService)
        {
            this._context = context;
            this._jwtService = jwtService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public IActionResult Login([FromBody] LoginDto response)
        {
            var user = this._context.Users.SingleOrDefault(x => x.Email == response.Email);
            if (user == null)
            {
                return NotFound();
            }

            var passwordValidation = BCrypt.Net.BCrypt.Verify(response.Password, user.Password);
            if (!passwordValidation)
            {
                return BadRequest();
            }

            var tokens = this._jwtService.Authenticate(user);

            if (tokens == null)
                return Unauthorized();
            
            return Ok(tokens);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public IActionResult Register([FromBody] RegisterDto response)
        {
            if (this._context.Users.Any(x => x.Email == response.Email))
            {
                return BadRequest("User with current email already existing.");
            }

            var user = new User()
            {
                Name = response.Name,
                Email = response.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(response.Password),
                Role = RoleTypes.User
            };

            this._context.Users.Add(user);
            this._context.SaveChanges();
            
            return Created("", new { status = "User was created successfully.", User = user });
        }
        
        [HttpPost("refresh")]
        [AllowAnonymous]
        public IActionResult Refresh([FromBody] RefreshCredentials refreshCredentials)
        {
            var tokens = this._jwtService.Refresh(refreshCredentials);

            if (tokens == null)
                return Unauthorized();

            return Ok(tokens);
        }
    }
}