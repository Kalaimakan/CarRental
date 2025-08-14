namespace CarRental.DTOs
{
    public class MyBookingListItemDTO
    {
        public Guid BookingId { get; set; }
        public string? CarBrandAndModel { get; set; } 
        public string? CarImageUrl { get; set; } 
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string? BookingStatus { get; set; } 
    }
}
