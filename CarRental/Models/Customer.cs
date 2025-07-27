using System.ComponentModel.DataAnnotations;

namespace CarRental.Models
{
    public class Customer
    {
        [Key]
        public Guid Id { get; set; }
        [Required(ErrorMessage = "Customer name is required")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Name must be between 2 and 100 characters")]
        public string Name { get; set; }
        public string Email { get; set; }
        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        [StringLength(20)]
        public string PhoneNumber { get; set; }
        public string LicenceNumber { get; set; }
        public string Address { get; set; }

    }
}
