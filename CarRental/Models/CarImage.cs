using System.ComponentModel.DataAnnotations;

namespace CarRental.Models
{
    public class CarImage
    {
        [Key] public Guid Id { get; set; }

        [Required] public string FileName { get; set; }

        [Required] public Guid CarId { get; set; }
        public Car Car { get; set; }
    }
}
