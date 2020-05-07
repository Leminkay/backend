using System.ComponentModel.DataAnnotations;

namespace BankApp.DTO
{
    public class RegistrationCredentials
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }
        
    }
}