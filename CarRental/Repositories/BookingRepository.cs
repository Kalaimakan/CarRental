using CarRental.Database;
using CarRental.Interfaces;
using CarRental.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly AppDbcontext _db;
        public BookingRepository(AppDbcontext db) => _db = db;

        public void Add(Booking booking)
        {
            _db.Bookings.Add(booking);
            _db.SaveChanges();
        }
        public Booking GetById(Guid id) =>
            _db.Bookings.Include(b => b.Car).Include(b => b.Customer).FirstOrDefault(b => b.Id == id);

        public IEnumerable<Booking> GetByCustomerId(Guid customerId) =>_db.Bookings
       .Include(b => b.Car)
       .Include(b => b.Customer)
       .Where(b => b.CustomerId == customerId)
       .ToList();

        public IEnumerable<Booking> GetAll() =>
            _db.Bookings.Include(b => b.Car).Include(b => b.Customer).ToList();


        public void Update(Booking booking)
        {
            _db.Bookings.Update(booking);
            _db.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var booking = _db.Bookings.Find(id);
            if (booking != null)
            {
                _db.Bookings.Remove(booking);
                _db.SaveChanges();
            }
        }
    }
}
