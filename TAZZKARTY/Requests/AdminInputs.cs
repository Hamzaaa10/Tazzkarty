using System.ComponentModel.DataAnnotations;
using TAZZKARTY.Models;

namespace TAZZKARTY.Requests
{
    public class AdminInputs
    {
        [Required]
        [MinLength(13 ,ErrorMessage = "National ID cannot be lessthan than 13 numbers.")]
        [MaxLength(13, ErrorMessage = "National ID cannot be morethan than 13 numbers.")]
        public string NationalId { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Full Name cannot be longer than 100 characters.")]
        public string FullName { get; set; }
        [Required]
        [MinLength(11, ErrorMessage = "National Phone cannot be lessthan than 11 numbers.")]
        [MaxLength(11, ErrorMessage = "National Phone cannot be morethan than 11 numbers.")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(30, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 20 characters.")]
        [RegularExpression(@"(?=.*[A-Z])(?=.*[a-z])(?=.*[0-9])(?=.*[!@#$%^&*()_+{}\[\]:;'<>,.?~\\-]).+",
        ErrorMessage = "Password must contain at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Email is required.")]
        [RegularExpression("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$") ,]
        public string Email { get; set; }
        
        [Required]
        public IFormFile? Image { get; set; }
    }

}
