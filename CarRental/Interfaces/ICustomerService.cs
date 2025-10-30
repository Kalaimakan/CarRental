using CarRental.DTOs;
using CarRental.Models;
using CarRental.ViewModels;
using static CarRental.Services.Implementations.CustomerService;

namespace CarRental.Interfaces

{
    public interface ICustomerService
    {
        Task<CustomerDto> GetCustomerIdAsync(Guid id);
        Task<List<CustomerDto>> GetAllCustomersAsync();
        Task<DuplicateCheckResult> CheckDuplicateFieldsAsync(CustomerDto dto);
        Task AddCustomerAsync(CustomerDto dto);
        Task UpdateCustomerAsync(CustomerDto dto);
        Task UpdateCustomerProfileAsync(CustomerDto dto);
        Task DeleteCustomerAsync(Guid id);
        Task<List<CustomerDto>> SearchCustomersAsync(string searchTerm);
        Task<Customer> AuthenticateAsync(LoginViewModel loginVm);
    }
}
