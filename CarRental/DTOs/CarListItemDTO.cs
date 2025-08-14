namespace CarRental.DTOs
{
    public class CarListItemDTO
    {
        public Guid Id { get; set; }
        public string? BrandAndModel { get; set; } 
        public string? ImageUrl { get; set; } 
        public decimal PricePerDay { get; set; }
        public string? Status { get; set; } 
    }

}
