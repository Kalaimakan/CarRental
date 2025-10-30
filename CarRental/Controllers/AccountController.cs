using CarRental.DTOs;
using CarRental.Interfaces;
using CarRental.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CarRental.Controllers
{
    public class AccountController : Controller
    {
        private readonly ICustomerService _customerService;

        public AccountController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public IActionResult CustomerLogin()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CustomerLogin(LoginViewModel loginVm)
        {
            if (!ModelState.IsValid)
                return View(loginVm);

            var customer = await _customerService.AuthenticateAsync(loginVm);

            if (customer == null)
            {
                ModelState.AddModelError("", "Invalid username/email or password");
                return View(loginVm);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, customer.Name),               
                new Claim(ClaimTypes.Email, customer.Email),               
                new Claim("CustomerId", customer.Id.ToString())         
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return RedirectToAction("Index", "CustomerDashboard");
        }


        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("CustomerLogin");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var customerIdClaim = User.FindFirst("CustomerId");
            if (customerIdClaim == null) return RedirectToAction("CustomerLogin", "Account");

            var customerId = Guid.Parse(customerIdClaim.Value);
            var customer = await _customerService.GetCustomerIdAsync(customerId);

            var vm = new UpdateProfileViewModel
            {
                Id = customer.Id,
                Name = customer.Name,
                UserName = customer.UserName,
                PhoneNumber = customer.PhoneNumber,
                Address = customer.Address
            };

            return View(vm);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Profile(UpdateProfileViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var customerIdClaim = User.FindFirst("CustomerId");
            if (customerIdClaim == null) return RedirectToAction("CustomerLogin", "Account");

            var customerId = Guid.Parse(customerIdClaim.Value);
            var customer = await _customerService.GetCustomerIdAsync(customerId);

            if (customer == null)
            {
                ViewBag.ErrorMessage = "Customer not found!";
                return View(model);
            }

            var duplicates = await _customerService.CheckDuplicateFieldsAsync(new CustomerDto
            {
                Id = model.Id,
                UserName = model.UserName,
                PhoneNumber = model.PhoneNumber,
                Email = customer.Email,
                LicenceNumber = customer.LicenceNumber
            });

            if (duplicates.UserName && model.UserName != customer.UserName)
            {
                ViewBag.ErrorMessage = "This username is already taken.";
                return View(model);
            }

            if (duplicates.PhoneNumber && model.PhoneNumber != customer.PhoneNumber)
            {
                ViewBag.ErrorMessage = "This phone number is already taken.";
                return View(model);
            }

            if (!string.IsNullOrWhiteSpace(model.Name) && model.Name != customer.Name)
                customer.Name = model.Name;

            if (!string.IsNullOrWhiteSpace(model.UserName) && model.UserName != customer.UserName)
                customer.UserName = model.UserName;

            if (!string.IsNullOrWhiteSpace(model.PhoneNumber) && model.PhoneNumber != customer.PhoneNumber)
                customer.PhoneNumber = model.PhoneNumber;

            if (!string.IsNullOrWhiteSpace(model.Address) && model.Address != customer.Address)
                customer.Address = model.Address;

            if (!string.IsNullOrEmpty(model.NewPassword))
            {
                if (customer.Password != model.CurrentPassword)
                {
                    ViewBag.ErrorMessage = "Current password is incorrect!";
                    return View(model);
                }

                if (model.NewPassword != model.ConfirmPassword)
                {
                    ViewBag.ErrorMessage = "New and confirm password do not match!";
                    return View(model);
                }

                customer.Password = model.NewPassword;
            }

            await _customerService.UpdateCustomerProfileAsync(customer);

            ViewBag.SuccessMessage = "Profile updated successfully!";
            return View(model);
        }
    }
}
