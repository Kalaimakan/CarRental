using CarRental.DTOs;
using CarRental.Interfaces;
using CarRental.Models;
using CarRental.Repositories.Implementations;
using CarRental.ViewModels;

namespace CarRental.Services.Implementations
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repository;

        public CustomerService(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<CustomerDto>> GetAllCustomersAsync()
        {
            var customers = await _repository.GetAllCustomersAsync();
            return customers.Select(c => new CustomerDto
            {
                Id = c.Id,
                Name = c.Name,
                Address = c.Address,
                Email = c.Email,
                PhoneNumber = c.PhoneNumber,
                LicenceNumber = c.LicenceNumber,
                UserName = c.UserName,
                Password = c.Password
            }).ToList();
        }

        public async Task<CustomerDto> GetCustomerIdAsync(Guid id)
        {
            var c = await _repository.GetCustomerByIdAsync(id);
            if (c == null) return null;

            return new CustomerDto
            {
                Id = c.Id,
                Name = c.Name,
                PhoneNumber = c.PhoneNumber,
                Address = c.Address,
                Email = c.Email,
                LicenceNumber = c.LicenceNumber,
                UserName = c.UserName,
                Password = c.Password

            };
        }

        public class DuplicateCheckResult
        {
            public bool Email { get; set; }
            public bool UserName { get; set; }
            public bool PhoneNumber { get; set; }
            public bool LicenceNumber { get; set; }

            public bool HasAny() => Email || UserName || PhoneNumber || LicenceNumber;
        }

        public async Task<DuplicateCheckResult> CheckDuplicateFieldsAsync(CustomerDto dto)
        {
            var allCustomers = await _repository.GetAllCustomersAsync();

            return new DuplicateCheckResult
            {
                Email = allCustomers.Any(c => c.Email == dto.Email),
                UserName = allCustomers.Any(c => c.UserName == dto.UserName),
                PhoneNumber = allCustomers.Any(c => c.PhoneNumber == dto.PhoneNumber),
                LicenceNumber = allCustomers.Any(c => c.LicenceNumber == dto.LicenceNumber)
            };
        }


        public async Task AddCustomerAsync(CustomerDto dto)
        {
            var customer = new Customer
            {
                Id = dto.Id == Guid.Empty ? Guid.NewGuid() : dto.Id,
                Name = dto.Name,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                Email = dto.Email,
                LicenceNumber = dto.LicenceNumber,
                UserName = dto.UserName,
                Password = dto.Password
            };

            await _repository.AddCustomerAsync(customer);
        }

        public async Task UpdateCustomerAsync(CustomerDto dto)
        {
            var UpdateCustomer = new Customer
            {
                Id = dto.Id,
                Name = dto.Name,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                Email = dto.Email,
                LicenceNumber = dto.LicenceNumber,
                UserName = dto.UserName,
                Password = dto.Password
            };
            await _repository.UpdateCustomerAsync(UpdateCustomer);
        }

        public async Task UpdateCustomerProfileAsync(CustomerDto dto)
        {
            var UpdateCustomer = new Customer
            {
                Id = dto.Id,
                Name = dto.Name,
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                Email = dto.Email,
                LicenceNumber = dto.LicenceNumber,
                UserName = dto.UserName,
                Password = dto.Password
            };
            await _repository.UpdateCustomerProfileAsync(UpdateCustomer);
        }

        public async Task DeleteCustomerAsync(Guid id)
        {
            var customer = await _repository.GetCustomerByIdAsync(id);
            if (customer != null)
            {
                await _repository.DeleteCustomerAsync(customer);
            }
        }

        //public async Task<List<Customer>> SearchCustomersAsync(string searchTerm)
        //{
        //    return await _repository.SearchCustomersAsync(searchTerm);
        //}

        public async Task<List<CustomerDto>> SearchCustomersAsync(string searchTerm)
        {
            var customers = await _repository.SearchCustomersAsync(searchTerm);

            return customers.Select(c => new CustomerDto
            {
                Id = c.Id,
                Name = c.Name,
                Email = c.Email,
                PhoneNumber = c.PhoneNumber,
                Address = c.Address,
                LicenceNumber = c.LicenceNumber
            }).ToList();
        }
        public async Task<Customer> AuthenticateAsync(LoginViewModel loginVm)
        {
            var customer = await _repository.GetByEmailOrUsernameAsync(loginVm.Identifier);

            if (customer == null)
                return null;

            if (customer.Password == loginVm.Password)
                return customer;

            return null;
        }
    }
}