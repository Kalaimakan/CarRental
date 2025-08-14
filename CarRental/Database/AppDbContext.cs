using CarRental.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        // --- INGADHAAN PUTHU CODE-AH SERKANUM ---
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // "Cars" table-kku indha data-va aarambathulaye serthudu-nu solrom
            modelBuilder.Entity<Car>().HasData(
                new Car
                {
                    Id = Guid.Parse("94d64bbe-ca9f-4de2-91c6-fc065a646c8f"),
                    Brand = "Toyota",
                    Model = "Camry",
                    Colour = "Red",
                    PricePerDay = 3000,
                    Status = "Available",
                    Image = "/images/toyota_camry.jpg",
                    Description = "A comfortable sedan for family trips."
                },
                new Car
                {
                    Id = Guid.Parse("1e8f3a5b-7c4d-4e9a-8b1f-9c0a6d5e7f2a"),
                    Brand = "Ford",
                    Model = "Ecosport",
                    Colour = "Orange",
                    PricePerDay = 2500,
                    Status = "Available",
                    Image = "/images/ford_ecosport.jpg",
                    Description = "A compact SUV for city and long drives."
                },
                new Car
                {
                    Id = Guid.Parse("f2c9e8d7-6b5a-4e3d-9a8c-1f0b2e3d4c5b"),
                    Brand = "Hyundai",
                    Model = "i20",
                    Colour = "Red",
                    PricePerDay = 2000,
                    Status = "Booked",
                    Image = "/images/hyundai_i20.jpg",
                    Description = "A stylish hatchback perfect for the city."
                }
            );
        }
    }
}
