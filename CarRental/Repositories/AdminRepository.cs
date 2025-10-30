using CarRental.Database;
using CarRental.Interfaces;
using CarRental.Models;
using Microsoft.EntityFrameworkCore;

namespace CarRental.Repositories
{
    public class AdminRepository : IAdminRepository
    {
        private readonly AppDbcontext _context;

        public AdminRepository(AppDbcontext context)
        {
            _context = context;
        }

        public async Task<Admin> GetByEmailOrUsernameAsync(string identifier, string password)
        {
            return await _context.Admins
                .FirstOrDefaultAsync(a =>
                    (a.Email == identifier || a.Username == identifier) &&
                    a.Password == password
                );
        }
    }
}
