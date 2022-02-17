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
    public class ProductsController : ControllerBase
    {
        private readonly ILogger<ProductsController> _logger;
        private readonly SqlContext _context;

        public ProductsController(ILogger<ProductsController> logger, SqlContext context)
        {
            _logger = logger;
            _context = context;
        }

        [HttpGet(Name ="GetAllProducts")]
        [UseApiKey]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts(string? category)
        {
            IQueryable<ProductEntity> productQuery = _context.Products.Include(p => p.Category);

            if (category != null)
            {
                productQuery = productQuery.Where(p => p.Category.Name == category);
            }

            var productEntities = await productQuery.ToListAsync();
            List<ProductDto> productDtos = new List<ProductDto>();

            foreach(var productEntity in productEntities)
            {
                productDtos.Add(new ProductDto
                {
                    Id = productEntity.Id,
                    ArticleNumber = productEntity.ArticleNumber,
                    Name = productEntity.Name,
                    Description = productEntity.Description,
                    Price = productEntity.Price,
                    Category = productEntity.Category.Name,
                });
            }
            return Ok(productDtos);
        }

        [HttpGet("{id}")]
        [UseApiKey]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var productEntity = await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);

            if (productEntity == null)
            {
                return NotFound();
            }
            var productDto = new ProductDto
            {
                Id = productEntity.Id,
                ArticleNumber = productEntity.ArticleNumber,
                Name = productEntity.Name,
                Description = productEntity.Description,
                Price = productEntity.Price,
                Category = productEntity.Category.Name,
            };
            
            return Ok(productDto);
        }
        
        [HttpPost(Name = "AddProduct")]
        [UseAdminKey]
        public async Task<ActionResult<ProductDto>> AddProduct(AddProductDto addProductDto)
        {
            if(await _context.Products.AnyAsync(p => p.ArticleNumber == addProductDto.ArticleNumber))
            {
                return BadRequest();
            }
            var productCategory = await _context.ProductCategories.FirstOrDefaultAsync(p => p.Name == addProductDto.Category);
            ProductEntity productEntity;

            if (productCategory == null)
            {
                productEntity = new ProductEntity
                {
                    ArticleNumber = addProductDto.ArticleNumber,
                    Name = addProductDto.Name,
                    Description = addProductDto.Description,
                    Price = addProductDto.Price,
                    Category = new ProductCategoryEntity
                    {
                        Name = addProductDto.Category
                    }
                };
            }
            else
            {
                productEntity = new ProductEntity
                {
                    ArticleNumber = addProductDto.ArticleNumber,
                    Name = addProductDto.Name,
                    Description = addProductDto.Description,
                    Price = addProductDto.Price,
                    ProductCategoryEntityId = productCategory.Id,
                };
            }

            _context.Products.Add(productEntity);
            await _context.SaveChangesAsync();

            var productDto = new ProductDto
            {
                Id = productEntity.Id,
                ArticleNumber = productEntity.ArticleNumber,
                Description = productEntity.Description,
                Price = productEntity.Price,
                Category = productEntity.Category.Name
            };

            return CreatedAtAction("GetProduct", new {id= productEntity.Id}, productDto);
        }

        [HttpPut("{id}")]
        [UseAdminKey]
        public async Task<ActionResult> UpdateProduct(int id, AddProductDto productDto)
        {
            var productEntity = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (productEntity == null)
            {
                return NotFound();
            }
           var productCategory = await _context.ProductCategories.FirstOrDefaultAsync(p => p.Name == productDto.Category);

            if (productCategory == null)
            {
                {
                    productEntity.ArticleNumber = productDto.ArticleNumber;
                    productEntity.Name = productDto.Name;
                    productEntity.Description = productDto.Description;
                    productEntity.Price = productDto.Price;
                    productEntity.Category = new ProductCategoryEntity
                    {
                        Name = productDto.Category
                    };
                };
            }
            else
            {
                {
                    productEntity.ArticleNumber = productDto.ArticleNumber;
                    productEntity.Name = productDto.Name;
                    productEntity.Description = productDto.Description;
                    productEntity.Price = productDto.Price;
                    productEntity.ProductCategoryEntityId = productCategory.Id;
                }
            }
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        [UseAdminKey]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            var productEntity = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (productEntity == null)
            {
                return BadRequest();
            }
            _context.Products.Remove(productEntity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
