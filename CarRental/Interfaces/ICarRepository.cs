using CarRental.DTOs;
using CarRental.Models;

namespace CarRental.Interfaces
{
    public interface ICarRepository
    {
        void Add(Car car);
        void Update(Car car);
        void Delete(Car car);
        Car GetById(Guid id);
        List<Car> GetAll();
        Task<List<Car>> SearchCarsAsync(string? brand, string? model, string? status);
    }

}
