using System.ComponentModel.DataAnnotations;

namespace CarRental.Models
{
    public class Admin
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Username { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
