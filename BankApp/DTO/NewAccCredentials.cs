using System.ComponentModel.DataAnnotations;

namespace BankApp.DTO
{
    public class NewAccCredentials
    {
        [Required]
        public string Email { get; set; }
    }
}