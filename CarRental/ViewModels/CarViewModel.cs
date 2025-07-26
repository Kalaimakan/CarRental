using CarRental.Models;
using System.ComponentModel.DataAnnotations;

namespace CarRental.ViewModels
{
    public class CarViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Car Brand is required")]
        public string CarBrand { get; set; }

        [Required(ErrorMessage = "Car Model is required")]
        public string CarModel { get; set; }

        public string CarColour { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0, 100000, ErrorMessage = "Enter a valid price")]
        public int PricePerDay { get; set; }

        public bool Status { get; set; }

        public string Description { get; set; }

        [Display(Name = "Upload Car Images")]
        public List<IFormFile> ImageFiles { get; set; } = new();
        public List<CarImage> ExistingImages { get; set; } = new();

    }
}
