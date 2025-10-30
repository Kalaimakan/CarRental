using CarRental.Interfaces;
using CarRental.Models;
using CarRental.Services;
using CarRental.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers
{
    public class BookingController : Controller
    {
        private readonly ICarService _carService;
        private readonly IBookingService _service;

        public BookingController(ICarService carService, IBookingService service)
        {
            _carService = carService;
            _service = service;
        }

        [HttpGet]
        public IActionResult CreateBooking(Guid carId)
        {
            var car = _carService.GetCarById(carId);
            if (car == null) return NotFound();

            var BookingViewModel = new BookingViewModel
            {
                CarId = car.Id,
                CarBrand = car.CarBrand,
                CarModel = car.CarModel,
                CarImage = car.Images.FirstOrDefault()?.FileName ?? "default.png",
                PricePerDay = car.PricePerDay
            };
            return View(BookingViewModel);
        }

        [HttpPost]
        public IActionResult CreateBooking(BookingViewModel bookingViewModel)
        {
            if (!ModelState.IsValid)
                return View(bookingViewModel);

            var customerIdClaim = User.FindFirst("CustomerId");
            if (customerIdClaim == null) return RedirectToAction("Login", "Account");

            bookingViewModel.CustomerId = Guid.Parse(customerIdClaim.Value);

            _service.CreateBooking(bookingViewModel);

            TempData["BookingSuccess"] = "🎉 Booking Confirmed Successfully!";
            return RedirectToAction("MyBookings", "CustomerDashboard");
        }

        [HttpGet]
        public IActionResult GetAllBookings()
        {
            var bookings = _service.GetAll();
            return View(bookings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cancel(Guid id)
        {
            _service.CancelBooking(id);
            TempData["BookingCancelled"] = "Booking Cancelled!";
            return RedirectToAction("MyBookings", "CustomerDashboard");
        }
    }
}
