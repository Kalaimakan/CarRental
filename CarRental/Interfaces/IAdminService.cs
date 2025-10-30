using CarRental.Models;

namespace CarRental.Interfaces
{
    public interface IAdminService
    {
        Task<Admin> LoginAsync(string identifier, string password);
    }
}
