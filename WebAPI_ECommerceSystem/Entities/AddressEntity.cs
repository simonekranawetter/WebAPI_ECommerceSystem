using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI_ECommerceSystem.Entities
{
    public class AddressEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string Street { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string PostalCode { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string City { get; set; }

        public List<UserEntity> Users { get; set; }
        public List<OrderEntity> Orders { get; set; }

    }
}