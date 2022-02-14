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
        [Column(TypeName = "varchar(50)")]
        public DateTime OrderDate { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public OrderStatus Status { get; set; }



    }
}
