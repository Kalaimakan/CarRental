using CarRental.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Database
{
    public class AppDbcontext : DbContext
    {
        public AppDbcontext(DbContextOptions<AppDbcontext> options) : base(options)
        {
        }
        public DbSet<Admin> Admins { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed default admin with 
            modelBuilder.Entity<Admin>().HasData(
                new Admin
                {
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Username = "Admin",
                    Email = "admin@carrental.com",
                    Password = "Admin@123"
                }
            );
        }

        public DbSet<Car> cars { get; set; }
        public DbSet<CarImage> CarImages { get; set; }
        public DbSet<Customer> customers { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Booking> Bookings { get; set; }


    }
}
