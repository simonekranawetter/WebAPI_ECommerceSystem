using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using WebAPI_ECommerceSystem.DTO;
using WebAPI_ECommerceSystem.Entities;
using WebAPI_ECommerceSystem.Filters;

namespace WebAPI_ECommerceSystem.Controllers
{
    [Route("api/users")]
    [ApiController]

    [UseApiKey]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;
        private readonly SqlContext _context;

        public UsersController(ILogger<UsersController> logger, SqlContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet(Name = "GetAllUsers")]
        [Authorize]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetUsers()
        {
            var usersEntities = await _context.Users.ToListAsync();

            List<UserDto> usersDto = new List<UserDto>();

            foreach (var userEntity in usersEntities)
            {
                var userDto = new UserDto
                {
                    Id = userEntity.Id,
                    FirstName = userEntity.FirstName,
                    LastName = userEntity.LastName,
                };

                usersDto.Add(userDto);
            }
            return Ok(usersDto);
        }

        [HttpGet("{id}", Name = "GetUser")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetUser(int id)
        {
            var userEntity = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if(userEntity == null)
            {
                return NotFound();
            }
            var userDto = new UserDto
            {
                Id = userEntity.Id,
                FirstName = userEntity.FirstName,
                LastName = userEntity.LastName
            };
            return Ok(userDto);
        }

        [HttpPost(Name ="AddUser")]
        public async Task<ActionResult<UserDto>> AddUser(AddUserDto user)
        {
            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
            {
                return BadRequest();
            }

            var addressEntity = await _context.Addresses.FirstOrDefaultAsync(a => a.Street == user.Street && a.PostalCode == user.PostalCode && a.City == user.City);

            var passwordSalt = Guid.NewGuid().ToString();
            var saltyPassword = $"{user.Password}{passwordSalt}";

            string passwordHash;
            using (SHA512 sha512Hash = SHA512.Create())
            {
                byte[] sourceBytes = Encoding.UTF8.GetBytes(saltyPassword);
                byte[] hashBytes = sha512Hash.ComputeHash(sourceBytes);
                passwordHash = BitConverter.ToString(hashBytes).Replace("-",string.Empty);
            }

            UserEntity userEntity;
            if(addressEntity == null)
            {
                userEntity = new UserEntity
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Mobile = user.Mobile,
                    Phone = user.Phone,
                    PasswordSalt = passwordSalt,
                    PasswordHash = passwordHash,
                    Address = new AddressEntity
                    {
                        Street = user.Street,
                        PostalCode = user.PostalCode,
                        City = user.City
                    }
                };
            }
            else
            {
                userEntity = new UserEntity
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Mobile = user.Mobile,
                    Phone = user.Phone,
                    PasswordSalt = passwordSalt,
                    PasswordHash = passwordHash,
                    AddressId = addressEntity.Id
                };
            }
            _context.Users.Add(userEntity);
            await _context.SaveChangesAsync();

            var userDto = new UserDto
            {
                Id = userEntity.Id,
                FirstName = userEntity.FirstName,
                LastName = userEntity.LastName
            };
            return CreatedAtAction("GetUser", new {id = userEntity.Id}, userDto);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserDto user)
        {
            var userEntity = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if(userEntity == null)
            {
                return NotFound();
            }

            userEntity.FirstName = user.FirstName;
            userEntity.LastName = user.LastName;

            await _context.SaveChangesAsync();

            return NoContent();

        }
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var userEntity = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            if(userEntity == null)
            {
                return NotFound();
            }
            _context.Users.Remove(userEntity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
