using System.ComponentModel.DataAnnotations;

namespace WebAPI_ECommerceSystem.DTO
{
    public class AddProductDto
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string ArticleNumber { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [StringLength(1200, MinimumLength = 2)]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [StringLength(50, MinimumLength = 2)]
        public string Category { get; set; }
    }
}
