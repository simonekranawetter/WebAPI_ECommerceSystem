using System.ComponentModel.DataAnnotations;

namespace WebAPI_ECommerceSystem.Entities
{
    public class OrderRowEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int Amount { get; set; }
        public ProductEntity Product { get; set; }
        public int ProductEntityId { get; set; }
    }
}
