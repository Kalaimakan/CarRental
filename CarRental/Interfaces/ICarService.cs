using CarRental.DTOs;
using CarRental.Models;
using CarRental.ViewModels;

namespace CarRental.Interfaces
{
    public interface ICarService
    {
        void AddCar(CarViewModel model);
        void UpdateCar(CarViewModel model);
        void DeleteCar(Guid id);
        List<CarDto> GetAllCars();
        CarDto GetCarById(Guid id);
        Task<List<Car>> SearchCarsAsync(string? brand, string? model, string? status);
    }

}
