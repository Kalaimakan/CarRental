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
                .AsNoTracking() 
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
            if (!ModelState.IsValid)
                return View(model);

            var existingImages = dbContext.CarImages.Where(i => i.CarId == model.Id).ToList();

            foreach (var image in existingImages)
            {
                var imagePath = Path.Combine(env.WebRootPath, "images", image.FileName);
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath); 
                }

                dbContext.CarImages.Remove(image); 
            }

            var car = new Car
            {
                Id = model.Id,
                CarBrand = model.CarBrand,
                CarModel = model.CarModel,
                CarColour = model.CarColour,
                PricePerDay = model.PricePerDay,
                Status = model.Status,
                Description = model.Description
            };

            dbContext.Attach(car);
            dbContext.Entry(car).Property(c => c.CarBrand).IsModified = true;
            dbContext.Entry(car).Property(c => c.CarModel).IsModified = true;
            dbContext.Entry(car).Property(c => c.CarColour).IsModified = true;
            dbContext.Entry(car).Property(c => c.PricePerDay).IsModified = true;
            dbContext.Entry(car).Property(c => c.Status).IsModified = true;
            dbContext.Entry(car).Property(c => c.Description).IsModified = true;

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

                        dbContext.CarImages.Add(new CarImage
                        {
                            Id = Guid.NewGuid(),
                            FileName = fileName,
                            CarId = car.Id
                        });
                    }
                }
            }

            try
            {
                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Save failed: " + ex.Message);
                return BadRequest("Error: " + ex.Message);
            }

            TempData["Success"] = "✅ Car updated successfully!";
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
