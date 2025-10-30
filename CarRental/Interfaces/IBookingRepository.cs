using CarRental.Models;

namespace CarRental.Interfaces
{
    public interface IBookingRepository
    {
        void Add(Booking booking);
        Booking GetById(Guid id);
        IEnumerable<Booking> GetByCustomerId(Guid customerId);
        IEnumerable<Booking> GetAll();
        void Update(Booking booking);
        void Delete(Guid id);
    }
}
