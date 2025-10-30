namespace CarRental.DTOs
{
    public class CarDto
    {
        public Guid Id { get; set; }
        public string CarBrand { get; set; }
        public string CarModel { get; set; }
        public string CarColour { get; set; }
        public int Seats { get; set; }
        public string FuelType { get; set; }
        public string Transmission { get; set; }
        public bool HasAC { get; set; }
        public int PricePerDay { get; set; }
        public bool Status { get; set; }
        public string Description { get; set; }
        public List<CarImageDto> Images { get; set; } = new();
    }
}
