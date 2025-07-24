using System.ComponentModel.DataAnnotations;

namespace CarRental.Models
{
    public class Car
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string CarBrand { get; set; }

        [Required]
        public string CarModel { get; set; }

        public string CarColour { get; set; }

        [Required]
        public int PricePerDay { get; set; }

        public bool Status { get; set; }

        public string Description { get; set; }

        public ICollection<CarImage> Images { get; set; } = new List<CarImage>();
    }
}
