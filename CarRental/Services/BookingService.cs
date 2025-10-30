using CarRental.DTOs;
using CarRental.Interfaces;
using CarRental.Models;
using CarRental.Repositories;
using CarRental.ViewModels;

namespace CarRental.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _repo;
        private readonly ICarRepository _carRepository;

        public BookingService(IBookingRepository repo, ICarRepository carRepository)
        {
            _repo = repo;
            _carRepository = carRepository;
        }

        public void CreateBooking(BookingViewModel vm)
        {
            var days = (vm.DropOffDate - vm.PickupDate).Days;
            if (days <= 0) throw new Exception("Drop-off date must be after pickup date");

            decimal pricePerDay = 100;
            decimal total = days * pricePerDay;

            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                CustomerId = vm.CustomerId,
                CarId = vm.CarId,
                PickupDate = vm.PickupDate,
                DropOffDate = vm.DropOffDate,
                PricePerDay = pricePerDay,
                TotalPrice = total,
                Status = "Active"
            };

            _repo.Add(booking);

            var car = _carRepository.GetById(vm.CarId);
            if (car != null)
            {
                car.Status = false; 
                _carRepository.Update(car);
            }
        }

        public BookingDto GetById(Guid id)
        {
            var b = _repo.GetById(id);
            if (b == null) return null;

            return new BookingDto
            {
                Id = b.Id,
                CustomerId = b.CustomerId,
                CarId = b.CarId,
                PickupDate = b.PickupDate,
                DropOffDate = b.DropOffDate,
                TotalPrice = b.TotalPrice,
                Status = b.Status,
                CarName = b.Car?.CarBrand + " " + b.Car?.CarModel,
                CustomerName = b.Customer?.Name
            };
        }

        public IEnumerable<BookingDto> GetByCustomerId(Guid customerId)
        {
            return _repo.GetByCustomerId(customerId).Select(b => new BookingDto
            {
                Id = b.Id,
                CustomerId = b.CustomerId,
                CarId = b.CarId,
                PickupDate = b.PickupDate,
                DropOffDate = b.DropOffDate,
                TotalPrice = b.TotalPrice,
                Status = b.Status,
                CarName = b.Car?.CarBrand + " " + b.Car?.CarModel,
                CustomerName = b.Customer?.Name
            });
        }

        public IEnumerable<BookingDto> GetAll()
        {
            return _repo.GetAll().Select(b => new BookingDto
            {
                Id = b.Id,
                CustomerId = b.CustomerId,
                CarId = b.CarId,
                PickupDate = b.PickupDate,
                DropOffDate = b.DropOffDate,
                TotalPrice = b.TotalPrice,
                Status = b.Status,
                CarName = b.Car?.CarBrand + " " + b.Car?.CarModel,
                CustomerName = b.Customer?.Name
            });
        }

        public void UpdateBooking(Guid id, BookingViewModel vm)
        {
            var booking = _repo.GetById(id);
            if (booking == null) throw new Exception("Booking not found");

            var days = (vm.DropOffDate - vm.PickupDate).Days;
            booking.PickupDate = vm.PickupDate;
            booking.DropOffDate = vm.DropOffDate;
            booking.TotalPrice = days * booking.PricePerDay;

            _repo.Update(booking);
        }

        public void CancelBooking(Guid bookingId)
        {
            var booking = _repo.GetById(bookingId);
            if (booking == null) return;

            booking.Status = "Cancelled";
            _repo.Update(booking);

            var car = _carRepository.GetById(booking.CarId);
            if (car != null)
            {
                car.Status = true;
                _carRepository.Update(car);
            }
        }

        public void ReleaseExpiredBookings()
        {
            var expired = _repo.GetAll()
                .Where(b => b.DropOffDate < DateTime.Now && b.Status == "Active")
                .ToList();

            foreach (var booking in expired)
            {
                booking.Status = "Completed";
                _repo.Update(booking);

                var car = _carRepository.GetById(booking.CarId);
                if (car != null)
                {
                    car.Status = true; // free again
                    _carRepository.Update(car);
                }
            }
        }
    }
}
