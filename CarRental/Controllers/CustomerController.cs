using CarRental.Database;
using CarRental.DTOs;
using CarRental.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CarRental.Controllers
{
    public class CustomerController : Controller
    {
        private readonly AppDbContext _context;

        public CustomerController(AppDbContext context)
        {
            _context = context;
        }

        // --- METHOD 1: INDEX (List all cars from DB) ---
        public async Task<IActionResult> Index()
        {
            var carsFromDb = await _context.Cars.ToListAsync();

            var carListForView = carsFromDb.Select(car => new CarListItemDTO
            {
                Id = car.Id,
                BrandAndModel = $"{car.Brand} {car.Model}",
                PricePerDay = car.PricePerDay,
                ImageUrl = car.Image,
                Status = car.Status
            }).ToList();

            return View(carListForView);
        }

        // --- METHOD 2: DETAILS (Get a specific car's details from DB) ---
        public async Task<IActionResult> Details(Guid id)
        {
            var carFromDb = await _context.Cars.FirstOrDefaultAsync(c => c.Id == id);
            if (carFromDb == null)
            {
                return NotFound();
            }

            var carDetailForView = new CarDetailDTO
            {
                Id = carFromDb.Id,
                Brand = carFromDb.Brand,
                Model = carFromDb.Model,
                Colour = carFromDb.Colour,
                PricePerDay = carFromDb.PricePerDay,
                ImageUrl = carFromDb.Image,
                Status = carFromDb.Status,
                Description = carFromDb.Description
            };

            return View(carDetailForView);
        }

        // --- METHOD 3: BOOK (Show the booking form for a car) ---
        public async Task<IActionResult> Book(Guid id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }

            ViewBag.CarId = car.Id;
            ViewBag.CarName = $"{car.Brand} {car.Model}";
            return View();
        }

        // --- METHOD 4: CONFIRM BOOKING (Save the new booking to the DB) ---
        [HttpPost]
        [ValidateAntiForgeryToken] // Security feature
        public async Task<IActionResult> ConfirmBooking(BookingRequestDTO bookingRequest)
        {
            // Find the car from DB to calculate the price
            var carToBook = await _context.Cars.FindAsync(bookingRequest.CarId);
            if (carToBook == null)
            {
                // This shouldn't happen if the form is correct, but it's a good safety check
                return BadRequest("The selected car is no longer available.");
            }

            // Calculate total price
            var numberOfDays = (bookingRequest.EndDate - bookingRequest.StartDate).Days;
            if (numberOfDays <= 0) numberOfDays = 1; // Minimum 1 day charge for same-day rentals
            var totalPrice = numberOfDays * carToBook.PricePerDay;

            // Create a new Booking entity to save to the database
            var newBooking = new Booking
            {
                Id = Guid.NewGuid(),
                CarId = bookingRequest.CarId,
                StartDate = bookingRequest.StartDate,
                EndDate = bookingRequest.EndDate,
                TotalPrice = totalPrice,
                Status = "Confirmed"
                // Later, you can add CustomerId here when you have user login
            };

            // Add the new booking to the context and save changes to the database
            _context.Bookings.Add(newBooking);
            await _context.SaveChangesAsync(); // This is the line that actually saves to the DB!

            // Redirect to a success page with the new booking ID
            return RedirectToAction("BookingSuccess", new { id = newBooking.Id });
        }

        // --- METHOD 5: BOOKING SUCCESS PAGE ---
        public IActionResult BookingSuccess(Guid id)
        {
            ViewBag.BookingId = id;
            return View();
        }

        // --- METHOD 6: MY BOOKINGS (Get all bookings from the DB) ---
        public async Task<IActionResult> MyBookings()
        {
            // Get all bookings from the Bookings table
            var bookingsFromDb = await _context.Bookings.OrderByDescending(b => b.StartDate).ToListAsync();

            // Convert the list of Booking entities to a list of DTOs for the view
            var myBookingsList = new List<MyBookingListItemDTO>();
            foreach (var booking in bookingsFromDb)
            {
                // For each booking, find the corresponding car
                var car = await _context.Cars.FindAsync(booking.CarId);
                if (car != null)
                {
                    myBookingsList.Add(new MyBookingListItemDTO
                    {
                        BookingId = booking.Id,
                        CarBrandAndModel = $"{car.Brand} {car.Model}",
                        CarImageUrl = car.Image,
                        StartDate = booking.StartDate,
                        EndDate = booking.EndDate,
                        TotalPrice = booking.TotalPrice,
                        BookingStatus = booking.Status
                    });
                }
            }

            return View(myBookingsList);
        }
    }
}