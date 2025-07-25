using CarRental.Database;
using CarRental.Models;
using CarRental.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Controllers
{
    public class CarController : Controller
    {
        private readonly AppDbContext dbContext;
        private readonly IWebHostEnvironment env;

        public CarController(AppDbContext context, IWebHostEnvironment env)
        {
            dbContext = context;
            this.env = env;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(CarViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var car = new Car
            {
                Id = Guid.NewGuid(),
                CarBrand = model.CarBrand,
                CarModel = model.CarModel,
                CarColour = model.CarColour,
                PricePerDay = (int)model.PricePerDay,
                Status = model.Status,
                Description = model.Description,
                Images = new List<CarImage>()
            };

           
            if (model.ImageFiles != null && model.ImageFiles.Count > 0)
            {
                foreach (var file in model.ImageFiles)
                {
                    if (file.Length > 0)
                    {
                        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                        var imagePath = Path.Combine(env.WebRootPath, "images", fileName);

                        using (var stream = new FileStream(imagePath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }

                        car.Images.Add(new CarImage
                        {
                            Id = Guid.NewGuid(),
                            FileName = fileName,
                            CarId = car.Id
                        });

                        
                        Console.WriteLine("✅ Saved Image: " + imagePath);
                    }
                }
            }

            dbContext.Cars.Add(car);
            dbContext.SaveChanges();

            TempData["Success"] = "Car added successfully!";
            return RedirectToAction("ViewCar");
        }



        [HttpGet]
        public IActionResult ViewCar()
        {
            var cars = dbContext.Cars
                .Include(c => c.Images)
                .ToList();

            return View(cars);
        }

        [HttpGet]
        public IActionResult Update(Guid id)
        {
            var car = dbContext.Cars
                .Include(c => c.Images)
                .FirstOrDefault(c => c.Id == id);

            if (car == null)
                return NotFound();

            var model = new CarViewModel
            {
                Id = car.Id,
                CarBrand = car.CarBrand,
                CarModel = car.CarModel,
                CarColour = car.CarColour,
                PricePerDay = car.PricePerDay,
                Status = car.Status,
                Description = car.Description,
                ExistingImages = car.Images.ToList() 
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Update(CarViewModel model)
        {
            var car = dbContext.Cars
                .Include(c => c.Images)
                .FirstOrDefault(c => c.Id == model.Id);

            if (car == null)
                return NotFound();

            car.CarBrand = model.CarBrand;
            car.CarModel = model.CarModel;
            car.CarColour = model.CarColour;
            car.PricePerDay = model.PricePerDay;
            car.Status = model.Status;
            car.Description = model.Description;

            if (model.ImageFiles != null && model.ImageFiles.Count > 0)
            {
                foreach (var file in model.ImageFiles)
                {
                    if (file.Length > 0)
                    {
                        var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                        var filePath = Path.Combine(env.WebRootPath, "images", fileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }

                        car.Images.Add(new CarImage
                        {
                            Id = Guid.NewGuid(),
                            FileName = fileName,
                            CarId = car.Id
                        });
                    }
                }
            }

            dbContext.SaveChanges();
            return RedirectToAction("ViewCar");
        }



        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            var car = dbContext.Cars
                .Include(c => c.Images)
                .FirstOrDefault(c => c.Id == id);

            if (car == null)
                return NotFound();

            foreach (var image in car.Images)
            {
                var filePath = Path.Combine(env.WebRootPath, "images", image.FileName);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            dbContext.Cars.Remove(car);
            dbContext.SaveChanges();

            return RedirectToAction("ViewCar");
        }

    }
}
