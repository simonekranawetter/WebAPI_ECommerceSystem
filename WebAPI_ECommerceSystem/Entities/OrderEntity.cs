using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI_ECommerceSystem.Entities
{
    public class OrderEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string Name { get; set; }

        [Required]
        //[Column(TypeName = "Date")] //Fix me?!
        public DateTime OrderDate { get; set; }

        [Required]
        //[Column(TypeName = "nvarchar(50)")]
        public OrderStatus Status { get; set; }

        public AddressEntity Address { get; set; }
        public IEnumerable<OrderRowEntity> OrderRows { get; set; }
    }
    public enum OrderStatus
    {
        New,
        Shipped,
        Delivered
    }
}
