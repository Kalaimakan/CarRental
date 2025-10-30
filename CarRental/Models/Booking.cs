using System.ComponentModel.DataAnnotations;

namespace CarRental.Models
{
    public class Booking
    {
        [Key]
        public Guid Id { get; set; }              
        public Guid CustomerId { get; set; }     
        public Guid CarId { get; set; }           
        public DateTime PickupDate { get; set; }  
        public DateTime DropOffDate { get; set; } 
        public decimal PricePerDay { get; set; } 
        public decimal TotalPrice { get; set; }   
        public string Status { get; set; }        

        // Navigation Properties
        public Customer Customer { get; set; }
        public Car Car { get; set; }
    }
}
