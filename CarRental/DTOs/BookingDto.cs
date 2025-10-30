namespace CarRental.DTOs
{
    public class BookingDto
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid CarId { get; set; }
        public DateTime PickupDate { get; set; }
        public DateTime DropOffDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }

        public string CarName { get; set; }
        public string CustomerName { get; set; }
    }
}
