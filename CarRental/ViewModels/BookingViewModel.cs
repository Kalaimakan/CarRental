using System.ComponentModel.DataAnnotations;

namespace CarRental.ViewModels
{
    public class BookingViewModel
    {
        [Required]
        public Guid CarId { get; set; }
        public string CarBrand { get; set; }
        public string CarModel { get; set; }
        public string CarImage { get; set; }
        public decimal PricePerDay { get; set; }
        public bool Status { get; set; }
        public decimal TotalAmount { get; set; }
        [Required]
        public Guid CustomerId { get; set; }
        [Required]
        public DateTime PickupDate { get; set; }
        [Required]
        public DateTime DropOffDate { get; set; }
    }
}
