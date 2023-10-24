using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using School_Web_API.Models;
using School_Web_API.Services;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace School_Web_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly SchoolDatabaseContext _dbContext;
        private readonly IConfiguration _configuration;

        public AuthController(SchoolDatabaseContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] User formInput)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool isDuplicate = await _dbContext.Users.AnyAsync(x => x.Username == formInput.Username);

                    if (isDuplicate)
                    {
                        return BadRequest(new { message = "User already exists" });
                    }

                    formInput.UserId = Guid.NewGuid();
                    formInput.Password = PasswordHasher.ComputeHash(formInput.Password, _configuration["SecretHashPepper"]);
                    _dbContext.Add(formInput);
                    _dbContext.SaveChanges();

                    return Ok(new { message = "Register successful" });
                }
                catch (Exception e)
                {
                    return Ok(new { message = "Register failed : " + e.Message });
                }
            }

            return BadRequest(new { message = "Invalid model state" });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] Login formInput)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    User user = await _dbContext.Users.FirstOrDefaultAsync(x => x.Username == formInput.Username && x.Password == PasswordHasher.ComputeHash(formInput.Password, _configuration["SecretHashPepper"]));

                    if (user == null)
                    {
                        return Unauthorized(new { message = "Invalid username or password" });
                    }

                    var authClaims = new List<Claim>
                    {
                       new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                       new Claim("UserId", user.UserId.ToString()),
                    };

                    var token = GenerateToken(authClaims);

                    return Ok(new
                    {
                        message = "Login successful",
                        token = token
                    });
                }
                catch (Exception e)
                {
                    return Ok(new { message = "Register failed: " + e.Message});
                }
            }

            return BadRequest(new { message = "Invalid model state" });
        }

        private string GenerateToken(IEnumerable<Claim> claims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTKey:Secret"]));
            var _TokenExpiryTimeInHour = Convert.ToInt64(_configuration["JWTKey:TokenExpiryTimeInHour"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["JWTKey:ValidIssuer"],
                Audience = _configuration["JWTKey:ValidAudience"],
                Expires = DateTime.UtcNow.AddHours(_TokenExpiryTimeInHour),
                SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
                Subject = new ClaimsIdentity(claims)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
