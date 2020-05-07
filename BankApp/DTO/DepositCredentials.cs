using System.ComponentModel.DataAnnotations;

namespace BankApp.DTO
{
    public class DepositCredentials
    {
        [Required]
        public string Id { get; set; }
        public double Value { get; set; }
    }
}