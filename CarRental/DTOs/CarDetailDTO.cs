namespace CarRental.DTOs
{
    public class CarDetailDTO
    {
        public Guid Id { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public string? Colour { get; set; }
        public string? ImageUrl { get; set; }
        public decimal PricePerDay { get; set; }
        public string? Status { get; set; }
        public string? Description { get; set; }
    }
}
