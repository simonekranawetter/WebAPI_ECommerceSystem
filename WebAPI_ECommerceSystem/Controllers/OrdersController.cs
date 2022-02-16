using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI_ECommerceSystem.DTO;
using WebAPI_ECommerceSystem.Entities;
using WebAPI_ECommerceSystem.Filters;

namespace WebAPI_ECommerceSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;
        private readonly SqlContext _context;

        public OrdersController(ILogger<OrdersController> logger, SqlContext context)
        {
            _logger = logger;
            _context = context;
        }
        [HttpGet(Name ="GetAllOrders")]
        [UseApiKey]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
        {
            var orderEntities = await _context.Orders.Include(o => o.Address).Include(o => o.OrderRows).ThenInclude(r => r.Product).ToListAsync();
            List<OrderDto> orderDtos = new List<OrderDto>();
            
            foreach (var orderEntity in orderEntities)
            {
                var rowDtos = new List<OrderRowDto>();
                foreach (var orderRowEntity in orderEntity.OrderRows)
                {
                    var orderRowDto = new OrderRowDto
                    {
                        Id = orderRowEntity.Id,
                        Amount = orderRowEntity.Amount,
                        Product = new OrderProductDto
                        {
                            ArticleNumber = orderRowEntity.Product.ArticleNumber,
                            Name = orderRowEntity.Product.Name,
                            Price = orderRowEntity.Product.Price,
                        }
                    };

                    rowDtos.Add(orderRowDto);
                }

                var orderDto = new OrderDto
                {
                    Id = orderEntity.Id,
                    Name = orderEntity.Name,
                    OrderDate = orderEntity.OrderDate,
                    Status = orderEntity.Status,
                    OrderRows = rowDtos,
                    Address = new AddressDto
                    {
                        Street = orderEntity.Address.Street,
                        PostalCode = orderEntity.Address.PostalCode,
                        City = orderEntity.Address.City,
                    }
                };

                orderDtos.Add(orderDto);
            }
            return Ok(orderDtos);
        }

        // Todo: implement get for order and other crud stuff here!
    }
}
