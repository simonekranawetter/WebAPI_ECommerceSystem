using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAPI_ECommerceSystem.Entities
{
    public class UserEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string FirstName { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string LastName { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Email { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Phone { get; set; }

        [Required]
        [Column(TypeName = "varchar(50)")]
        public string Mobile { get; set; }

        [Required]
        [Column(TypeName = "varchar(128)")]
        public string PasswordHash { get; set; }

        [Required]
        [Column(TypeName = "varchar(36)")]
        public string PasswordSalt { get; set; }

        public AddressEntity Address { get; set; }
        public int AddressId { get; set; }
    }
}
