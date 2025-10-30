using CarRental.DTOs;
using CarRental.Models;
using System.ComponentModel.DataAnnotations;

namespace CarRental.ViewModels
{
    public class CarViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Car Brand is required")] public string CarBrand { get; set; }
        [Required(ErrorMessage = "Car Model is required")] public string CarModel { get; set; }
        [Required(ErrorMessage = "Car Colour is required")] public string CarColour { get; set; }
        [Required(ErrorMessage = "Seats is required")] public int Seats { get; set; }
        [Required(ErrorMessage = "Fuel Type is required")] public string FuelType { get; set; }
        [Required(ErrorMessage = "Transmission is required")] public string Transmission { get; set; } 
        [Required(ErrorMessage = "HasAC is required")] public bool HasAC { get; set; }
        [Required(ErrorMessage = "Price is required")][Range(0, 100000, ErrorMessage = "Enter a valid price")] public int PricePerDay { get; set; }
        [Required(ErrorMessage = "Status is required")] public bool Status { get; set; }
        [Required(ErrorMessage = "Description is required")] public string Description { get; set; }

        public List<IFormFile> ImageFiles { get; set; }

        public List<string> ExistingImages { get; set; } = new List<string>();
    }
}

