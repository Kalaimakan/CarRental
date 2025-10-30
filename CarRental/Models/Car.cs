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

        [Required]
        public string CarColour { get; set; }

        [Required]
        public int Seats { get; set; }

        [Required]
        public string FuelType { get; set; }

        [Required]
        public string Transmission { get; set; } 

        [Required]
        public bool HasAC { get; set; }

        [Required]
        public int PricePerDay { get; set; }

        [Required]
        public bool Status { get; set; }

        [Required]
        public string Description { get; set; }



        [Required]
        public ICollection<CarImage> Images { get; set; } = new List<CarImage>();

    }
}
