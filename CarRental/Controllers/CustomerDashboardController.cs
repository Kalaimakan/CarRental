using CarRental.Interfaces;
using CarRental.Services;
using CarRental.Services.Implementations;
using CarRental.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers
{
    public class CustomerDashboardController : Controller
    {
        private readonly ICarService _carService;
        private readonly IBookingService _bookingService;


        public CustomerDashboardController(ICarService carService, IBookingService bookingService)
        {
            _carService = carService;
            _bookingService = bookingService;
        }

        [Authorize]
        public IActionResult Index()
        {
            var customerIdClaim = User.FindFirst("CustomerId");
            var customerId = customerIdClaim != null ? customerIdClaim.Value : null;
            var customerName = User.Identity?.Name;

            ViewBag.CustomerName = customerName;
            ViewBag.CustomerId = customerId;
            
            return View();
        }

        // Browse cars page
        public IActionResult ShowAllCars()
        {
            var cars = _carService.GetAllCars()
        .Where(c => c.Status == true) 
        .ToList();

            return View(cars);
        }

        // My bookings page
        public IActionResult MyBookings()
        {
            var customerIdClaim = User.FindFirst("CustomerId");
            if (customerIdClaim == null) return RedirectToAction("Login", "Account");

            var customerId = Guid.Parse(customerIdClaim.Value);

            var bookings = _bookingService.GetByCustomerId(customerId);
            return View(bookings); 
        }

        

    }
}