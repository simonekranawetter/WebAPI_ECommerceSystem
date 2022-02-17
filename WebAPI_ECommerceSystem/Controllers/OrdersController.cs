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
        [HttpGet(Name = "GetAllOrders")]
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

        [HttpGet("{id}")]
        [UseApiKey]
        public async Task<ActionResult<OrderDto>> GetOrder(int id)
        {
            var orderEntity = await _context.Orders.Include(o => o.Address).Include(o => o.OrderRows).ThenInclude(r => r.Product).FirstOrDefaultAsync();

            if (orderEntity == null)
            {
                return NotFound();
            }

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
                    City = orderEntity.Address.City
                }
            };

            return Ok(orderDto);
        }

        [HttpPost(Name = "AddOrder")]
        [UseApiKey]
        public async Task<ActionResult> AddOrder(CreateOrderDto orderDto)
        {
            List<OrderRowEntity> orderRowEntities = new List<OrderRowEntity>();

            foreach (var orderRowDto in orderDto.OrderRows)
            {
                var productEntity = await _context.Products.FirstOrDefaultAsync(p => p.ArticleNumber == orderRowDto.ArticleNumber);

                if (productEntity == null)
                {
                    return NotFound();
                }

                var orderRowEntity = new OrderRowEntity
                {
                    ProductEntityId = productEntity.Id,
                    Amount = orderRowDto.Amount
                };

                orderRowEntities.Add(orderRowEntity);
            }

            var addressEntity = await _context.Addresses.FirstOrDefaultAsync(a => a.Street == orderDto.Address.Street && a.PostalCode == orderDto.Address.PostalCode && a.City == orderDto.Address.City);
            OrderEntity orderEntity;

            if (addressEntity == null)
            {
                orderEntity = new OrderEntity
                {
                    OrderDate = DateTime.Now,
                    Status = OrderStatus.New,
                    Name = orderDto.Name,
                    Address = new AddressEntity
                    {
                        Street = orderDto.Address.Street,
                        PostalCode = orderDto.Address.PostalCode,
                        City = orderDto.Address.City
                    },
                    OrderRows = orderRowEntities
                };
            }
            else
            {
                orderEntity = new OrderEntity
                {
                    OrderDate = DateTime.Now,
                    Status = OrderStatus.New,
                    Name = orderDto.Name,
                    AddressEntityId = addressEntity.Id,
                    OrderRows = orderRowEntities
                };
            }

            _context.Orders.Add(orderEntity);
            await _context.SaveChangesAsync();

            var rowDtos = new List<OrderRowDto>();
            foreach (var orderRowEntity in orderEntity.OrderRows)
            {
                var orderRowDto = new OrderRowDto
                {
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

            var orderDto2 = new OrderDto
            {
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

            return CreatedAtAction("GetOrder", new { id = orderEntity.Id }, orderDto2);
        }

        [HttpPut("{id}/row/{rowid}")]
        [UseApiKey]
        public async Task<ActionResult> UpdateOrderRow(int id, int rowid, CreateOrderRowDto orderRowDto)
        {
            var orderRowEntity = await _context.OrderRows.FirstOrDefaultAsync(o => o.Id == rowid);

            if (orderRowEntity == null)
            {
                return NotFound();
            }

            var productEntity = await _context.Products.FirstOrDefaultAsync(p => p.ArticleNumber == orderRowDto.ArticleNumber);
            
            if (productEntity == null)
            {
                return NotFound();
            }

            orderRowEntity.Amount = orderRowDto.Amount;
            orderRowEntity.ProductEntityId = productEntity.Id;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}/status")]
        [UseAdminKey]
        public async Task<ActionResult> UpdateStatus(int id, UpdateOrderStatusDto updateOrderStatusDto)
        {
            var orderEntity = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);

            if(orderEntity == null)
            {
                return NotFound();
            }
            orderEntity.Status = updateOrderStatusDto.Status;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [UseAdminKey]
        public async Task<ActionResult> Delete(int id)
        {
            var orderEntity = await _context.Orders.Include(o => o.OrderRows).FirstOrDefaultAsync(o => o.Id == id);

            if (orderEntity == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(orderEntity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
