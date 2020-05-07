using System.ComponentModel.DataAnnotations;

namespace BankApp.DTO
{
    public class TransferCredentials
    {
        [Required]
        public string FromId { get; set; }
        [Required]
        public string ToId { get; set; }
        [Required]
        public double Value { get; set; }
    }
}