using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace WebAPI_ECommerceSystem.DTO
{
    public class AddUserDto
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; }

        [Required]
        [StringLength (50)]
        [RegularExpression(@"[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?", ErrorMessage ="Must be a valid email address.")]
        public string Email { get; set; }

        [Required]
        [StringLength (50)]
        [RegularExpression(@"^(?=.*?[A-Ö])(?=.*?[a-ö])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$", ErrorMessage = "Must be a valid password.")]
        public string Password { get; set; }

        [Required]
        [StringLength(50, MinimumLength =2)]
        [RegularExpression(@"\+?\d{2,}", ErrorMessage = "Must be a valid number.")]

        public string Phone { get; set; }

        [Required]
        [StringLength(50, MinimumLength =2)]
        [RegularExpression(@"\+?\d{2,}", ErrorMessage = "Must be a valid number.")]

        public string Mobile { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Street { get; set; }

        [Required]
        [StringLength (10, MinimumLength = 5)]
        [RegularExpression(@"^\d{3}(?:[ ]?\d{2})?$", ErrorMessage ="Must be a valid postal code.")]
        public string PostalCode { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string City { get; set; }
    }

}
