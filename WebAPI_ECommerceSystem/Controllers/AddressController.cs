using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI_ECommerceSystem.DTO;
using WebAPI_ECommerceSystem.Entities;
using WebAPI_ECommerceSystem.Filters;

namespace WebAPI_ECommerceSystem.Controllers
{
    [Route("api/users/{id}")]
    [ApiController]
    [Authorize]
    public class AddressController : ControllerBase
    {
        private readonly ILogger<AddressController> _logger;
        private readonly SqlContext _context;

        public AddressController(ILogger<AddressController> logger, SqlContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet("address")]
        [UseApiKey]
        public async Task<ActionResult<AddressDto>> GetAddressForUser(int id)
        {
            var userEntity = await _context.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Id == id);

            if (userEntity == null || userEntity.Address == null)
            {
                return NotFound();
            }

            var addressDto = new AddressDto
            {
                Street = userEntity.Address.Street,
                PostalCode = userEntity.Address.PostalCode,
                City = userEntity.Address.City,
            };
            return addressDto;
        }

        [HttpPut("address")]
        [UseApiKey]
        public async Task<IActionResult> UpdateAddress(int id, AddressDto address)
        {
            var userEntity = await _context.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Id == id);

            if (userEntity == null)
            {
                return NotFound();
            }

            var addressEntity = await _context.Addresses.FirstOrDefaultAsync(a => a.Street == address.Street && a.PostalCode == address.PostalCode && a.City == address.City);

            if (addressEntity == null)
            {
                userEntity.Address = new AddressEntity
                {
                    Street = address.Street,
                    PostalCode=address.PostalCode,
                    City=address.City
                };
            }
            else
            {
                userEntity.AddressId = addressEntity.Id;
            }

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
