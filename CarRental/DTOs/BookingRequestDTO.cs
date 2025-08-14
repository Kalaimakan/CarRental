namespace CarRental.DTOs
{
    public class BookingRequestDTO
    {
        public Guid CarId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
