namespace CarRental.Models
{
    public class Booking
    {
        public Guid Id { get; set; }
        public Guid CarId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; } = "Confirmed"; 
    }
}
