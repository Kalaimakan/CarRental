using CarRental.Models;

namespace CarRental.Interfaces
{
    public interface IAdminRepository
    {
        Task<Admin> GetByEmailOrUsernameAsync(string identifier, string password);
    }
}
