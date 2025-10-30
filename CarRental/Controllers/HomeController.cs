using CarRental.Interfaces;
using CarRental.Models;
using CarRental.Services;   // ✅ Add this namespace for ICarService
using CarRental.ViewModels;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace CarRental.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICarService _carService;
        private readonly IContactService _contactService;

        // ✅ Only one constructor with both dependencies
        public HomeController(ILogger<HomeController> logger, ICarService carService, IContactService contactService)
        {
            _logger = logger;
            _carService = carService;
            _contactService = contactService;
        }

        // ✅ Show all cars in home page
        public IActionResult Index()
        {

            var cars = _carService.GetAllCars();
            return View(cars);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult ContactView()
        {
            var contact = _contactService.GetAllContacts();
            return View(contact);
        }


        [HttpGet]
        public IActionResult Contact()
        {
            return View(new ContactViewModel());
        }

        [HttpPost]
        public IActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                _contactService.SendContact(model.Contact);
                model.SuccessMessage = "Your message has been sent successfully!";
                ModelState.Clear();
            }
            return View(model);
        }

        public IActionResult Gallery()
        {
            var cars = _carService.GetAllCars();

            if (cars == null)
                cars = new List<CarRental.DTOs.CarDto>();

            return View(cars);
        }

        public IActionResult AboutUs()
        {
            return View();
        }
    }
}
