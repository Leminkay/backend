using System.ComponentModel.DataAnnotations;

namespace BankApp.Models
{
    public class Users : BaseEntity
    {
        [Key]
        public long Id { get; set; }
        [Required]
        public string Name { get; set; }        
        [Required]
        public string Email { get; set; }        
        [Required]
        public string Salt { get; set; }        
        [Required]
        public string Password { get; set; }
    }
}