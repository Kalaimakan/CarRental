using CarRental.DTOs;
using CarRental.Interfaces;
using CarRental.Models;
using CarRental.Services;
using CarRental.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers
{
    public class CarController : Controller
    {
        private readonly ICarService service;
        public CarController(ICarService service) => this.service = service;

        [HttpGet]
        public IActionResult ViewCar()
        {
            var cars = service.GetAllCars();
            return View(cars);
        }

        [HttpGet]
        public IActionResult Add() => View();

        [HttpPost]
        public IActionResult Add(CarViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            service.AddCar(model);
            TempData["Success"] = "Car added successfully!";
            return RedirectToAction(nameof(ViewCar));
        }

        [HttpGet]
        public IActionResult Update(Guid id)
        {
            var car = service.GetCarById(id);
            if (car == null) return NotFound();

            var model = new CarViewModel
            {
                Id = car.Id,
                CarBrand = car.CarBrand,
                CarModel = car.CarModel,
                CarColour = car.CarColour,
                Seats = car.Seats,
                FuelType = car.FuelType,
                Transmission = car.Transmission,
                HasAC = car.HasAC,
                PricePerDay = car.PricePerDay,
                Status = car.Status,
                Description = car.Description,
                ExistingImages = car.Images.Select(i => i.FileName).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Update(CarViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                service.UpdateCar(model);
                TempData["Success"] = "Car updated successfully!";
                return RedirectToAction("ViewCar");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            service.DeleteCar(id);
            TempData["Success"] = "Car deleted successfully!";
            return RedirectToAction(nameof(ViewCar));
        }

        [HttpGet]
        public IActionResult Details(Guid id)
        {
            var car = service.GetCarById(id); // Service → Repo

            if (car == null) return NotFound();

            return View(car);
        }

        [HttpGet]
        public async Task<IActionResult> SearchCar(string? brand, string? model, string? status)
        {
            var cars = await service.SearchCarsAsync(brand, model, status);
            return View("SearchCar", cars);
        }
    }
}