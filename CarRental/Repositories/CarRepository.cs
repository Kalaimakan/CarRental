using CarRental.Database;
using CarRental.Interfaces;
using CarRental.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel;

namespace CarRental.Repositories
{
    public class CarRepository : ICarRepository
    {
        private readonly AppDbcontext _db;
        public CarRepository(AppDbcontext db) { _db = db; }

        public void Add(Car car)
        {
            _db.cars.Add(car);
            _db.SaveChanges();
        }

        public void Update(Car car)
        {
            try
            {
                _db.cars.Update(car);
                _db.SaveChanges();
            }
            catch (DbUpdateException dbEx)
            {
                Console.WriteLine("❌ Database Update Error: " + dbEx.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("❌ Unexpected Error: " + ex.Message);
            }
        }

        public void Delete(Car car)
        {
            _db.cars.Remove(car);
            _db.SaveChanges();
        }

        public Car GetById(Guid id) =>
            _db.cars.Include(c => c.Images).FirstOrDefault(c => c.Id == id);

        public List<Car> GetAll() =>
            _db.cars.Include(c => c.Images).ToList();

        public async Task<List<Car>> SearchCarsAsync(string? brand, string? model, string? status)
        {
            var query = _db.cars.AsQueryable();

            if (!string.IsNullOrWhiteSpace(brand))
                query = query.Where(c => c.CarBrand.ToLower().Contains(brand.ToLower()));

            if (!string.IsNullOrWhiteSpace(model))
                query = query.Where(c => c.CarModel.ToLower().Contains(model.ToLower()));

            if (!string.IsNullOrWhiteSpace(status))
                query = query.Where(c => c.Status.ToString().ToLower().Contains(status.ToLower()));

            return await query.Include(c => c.Images).ToListAsync();
        }

    }
}


