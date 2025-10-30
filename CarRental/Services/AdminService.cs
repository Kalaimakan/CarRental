using CarRental.Interfaces;
using CarRental.Models;

namespace CarRental.Services
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;

        public AdminService(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;
        }

        public async Task<Admin> LoginAsync(string identifier, string password)
        {
            return await _adminRepository.GetByEmailOrUsernameAsync(identifier, password);
        }
    }
}
