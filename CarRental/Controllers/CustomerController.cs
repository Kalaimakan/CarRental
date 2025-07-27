using CarRental.Database;
using CarRental.Models;
using CarRental.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;

namespace CarRental.Controllers
{
    public class CustomerController : Controller
    {
        private readonly AppDbContext dbContext;

        public CustomerController(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

       
      

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(CustomerViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }

           
            var customer = new Customer
            {
                Name = vm.Name,
                Email = vm.Email,
                PhoneNumber = vm.PhoneNumber,
                Address = vm.Address,
                LicenceNumber = vm.LicenceNumber
            };

            dbContext.Customers.Add(customer);
            dbContext.SaveChanges();

            return RedirectToAction("ViewCustomer");
        }

        [HttpGet]
        public IActionResult ViewCustomer()
        {
            var customers = dbContext.Customers.ToList();
            return View(customers);
        }


        [HttpGet]
        public IActionResult Update(Guid id)
        {
            var customer = dbContext.Customers.Find(id);

            if (customer == null)
            {
                return NotFound();
            }


            var customerViewModel = new CustomerViewModel
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                Address = customer.Address,
                LicenceNumber = customer.LicenceNumber
            };

            return View(customerViewModel);
        }

        [HttpPost]
        public IActionResult Update(Customer customer)
        {
            var existingCustomer = dbContext.Customers.Find(customer.Id);
            if (existingCustomer == null)
            {
                return NotFound();
            }


            existingCustomer.Name = customer.Name;
            existingCustomer.Email = customer.Email;
            existingCustomer.PhoneNumber = customer.PhoneNumber;
            existingCustomer.LicenceNumber = customer.LicenceNumber;
            existingCustomer.Address = customer.Address;

            dbContext.SaveChanges();

            return RedirectToAction("ViewCustomer");
            
        }



        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            var customer = dbContext.Customers.Find(id);

            if (customer == null)
            {
                return NotFound();
            }

            dbContext.Customers.Remove(customer);
            dbContext.SaveChanges();

            return RedirectToAction("ViewCustomer");
        }
    }
}
