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
    }
}
