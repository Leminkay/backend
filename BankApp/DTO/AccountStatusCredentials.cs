using System.ComponentModel.DataAnnotations;

namespace BankApp.DTO
{
    public class AccountStatusCredentials
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public  string Status { get; set; }
    }
}