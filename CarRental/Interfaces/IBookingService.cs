using CarRental.DTOs;
using CarRental.ViewModels;

namespace CarRental.Interfaces
{
    public interface IBookingService
    {
        void CreateBooking(BookingViewModel vm);
        BookingDto GetById(Guid id);
        IEnumerable<BookingDto> GetByCustomerId(Guid customerId);
        IEnumerable<BookingDto> GetAll();
        void UpdateBooking(Guid id, BookingViewModel vm);
        void CancelBooking(Guid id);
    }
}
