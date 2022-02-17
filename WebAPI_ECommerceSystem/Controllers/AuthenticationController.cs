using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebAPI_ECommerceSystem.DTO;
using WebAPI_ECommerceSystem.Filters;

namespace WebAPI_ECommerceSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [UseApiKey]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<AuthenticationController> _logger;
        private readonly IConfiguration _configuration;
        private readonly SqlContext _context;

        public AuthenticationController(ILogger<AuthenticationController> logger, IConfiguration configuration, SqlContext context)
        {
            _logger = logger;
            _configuration = configuration;
            _context = context;
        }

        [HttpPost("SignIn")]
        public async Task<ActionResult> SignIn(SignInDto signInDto)
        {
            if(string.IsNullOrEmpty(signInDto.Email) || string.IsNullOrEmpty(signInDto.Password))
            {
                return BadRequest("Email and Password must be provided");
            }
            var userEntity = await _context.Users.FirstOrDefaultAsync(x => x.Email == signInDto.Email);

            string passwordHash;
            var saltyPassword = $"{signInDto.Password}{userEntity.PasswordSalt}";
            using (SHA512 sha512 = SHA512.Create())
            {
                byte[] sourceBytes = Encoding.UTF8.GetBytes(saltyPassword);
                byte[] hashBytes = sha512.ComputeHash(sourceBytes);
                passwordHash = BitConverter.ToString(hashBytes).Replace("-", string.Empty);
            }

            var validPassword = passwordHash.Equals(userEntity.PasswordHash); //Test me!

            if (userEntity == null || !validPassword)
            {
                return BadRequest("Incorrect Email or Password");
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("id", userEntity.Id.ToString()),
                    new Claim(ClaimTypes.Name, userEntity.Email),
                    new Claim("code", _configuration.GetValue<string>("ApiKey"))
                }),
                Expires = DateTime.Now.AddMinutes(1),
                SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Secret"))),
                SecurityAlgorithms.HmacSha512Signature
                )
            };

            return Ok(tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor)));
        }
    }
}
