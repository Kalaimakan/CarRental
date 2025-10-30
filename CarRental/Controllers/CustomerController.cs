using CarRental.DTOs;
using CarRental.Interfaces;
using CarRental.Repositories.Implementations;
using CarRental.Services;
using CarRental.Services.Implementations;
using CarRental.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace CarRental.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _service;
        private readonly IOtpService _otpService;
        private readonly IEmailService _emailService;

        public CustomerController(ICustomerService customerService, IOtpService otpService, IEmailService emailService)
        {
            _service = customerService;
            _otpService = otpService;
            _emailService = emailService;
        }

        //Testing Gmail OTP

        //[HttpGet]
        //public IActionResult TestEmail()
        //{
        //    try
        //    {
        //        _emailService.SendEmail("ut01482tic2024@gmail.com", "Test Email", "Hello, this is a test!");
        //        return Content("Email sent successfully!");
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content("Email sending failed: " + ex.Message);
        //    }
        //}


        //register page
        public IActionResult RegisterCustomer()
        {
            return View();
        }


        //register Customer
        [HttpPost]
        public async Task<IActionResult> RegisterCustomer(CustomerViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var dto = new CustomerDto
            {
                Name = model.Name,
                Address = model.Address,
                PhoneNumber = model.PhoneNumber,
                LicenceNumber = model.LicenceNumber,
                Email = model.Email,
                UserName = model.Username,
                Password = model.Password
            };

            // Check which fields are duplicates
            var duplicates = await _service.CheckDuplicateFieldsAsync(dto);

            if (duplicates.Email)
                ModelState.AddModelError("Email", "Email already taken");

            if (duplicates.UserName)
                ModelState.AddModelError("Username", "Username already taken");

            if (duplicates.PhoneNumber)
                ModelState.AddModelError("PhoneNumber", "Phone number already taken");

            if (duplicates.LicenceNumber)
                ModelState.AddModelError("LicenceNumber", "Licence number already taken");

            if (duplicates.HasAny())
                return View(model);


            var otp = _otpService.GenerateOtp(model.Email);

            try
            {
                _emailService.SendEmail(model.Email, "Welcome! Your OTP Code", $"Hello {model.Name}, your OTP is: {otp}");
                Console.WriteLine($"[DEBUG] OTP generated for {model.Email}: {otp}");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Could not send OTP email: " + ex.Message);
                return View(model);
            }
            TempData["RegisterData"] = JsonSerializer.Serialize(model);
            return RedirectToAction("VerifyOtpRegister", new { email = model.Email });
        }


        [HttpGet]
        public IActionResult VerifyOtpRegister(string email)
        {
            return View(new OtpViewModel { Email = email });
        }


        [HttpPost]
        public async Task<IActionResult> VerifyOtpRegister(OtpViewModel model)
        {
            if (_otpService.VerifyOtp(model.Email, model.Otp))
            {
                var dtoJson = TempData["RegisterData"] as string;
                if (dtoJson != null)
                {
                    var registerData = JsonSerializer.Deserialize<CustomerViewModel>(dtoJson);
                    var dto = new CustomerDto
                    {
                        Name = registerData.Name,
                        Address = registerData.Address,
                        PhoneNumber = registerData.PhoneNumber,
                        LicenceNumber = registerData.LicenceNumber,
                        Email = registerData.Email,
                        UserName = registerData.Username,
                        Password = registerData.Password
                    };
                    await _service.AddCustomerAsync(dto);
                }
                return RedirectToAction("CustomerLogin", "Account");
            }
            ModelState.AddModelError("", "Invalid OTP");
            return View(model);
        }

        public IActionResult Success()
        {
            return View();
        }

        //View Customer
        public async Task<IActionResult> ViewCustomer()
        {
            var customer = await _service.GetAllCustomersAsync();
            return View(customer);
        }

        //otp for add
        public IActionResult Add()
        {
            return View();
        }

        //Add Customer
        [HttpPost]
        public async Task<IActionResult> Add(CustomerDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var Customerdto = new CustomerDto
            {
                Name = dto.Name,
                Address = dto.Address,
                PhoneNumber = dto.PhoneNumber,
                LicenceNumber = dto.LicenceNumber,
                Email = dto.Email,
                UserName = dto.UserName,
                Password = dto.Password
            };

            // Check which fields are duplicates
            var duplicates = await _service.CheckDuplicateFieldsAsync(Customerdto);

            if (duplicates.Email)
                ModelState.AddModelError("Email", "Email already taken");

            if (duplicates.UserName)
                ModelState.AddModelError("UserName", "Username already taken");

            if (duplicates.PhoneNumber)
                ModelState.AddModelError("PhoneNumber", "Phone number already taken");

            if (duplicates.LicenceNumber)
                ModelState.AddModelError("LicenceNumber", "Licence number already taken");

            if (duplicates.HasAny())
                return View(dto);


            var otp = _otpService.GenerateOtp(dto.Email);
            TempData["AdminregisterData"] = JsonSerializer.Serialize(dto);

            try
            {
                _emailService.SendEmail(dto.Email, "Welcome! Your OTP Code", $"Hello {dto.Name}, your OTP is: {otp}");
                Console.WriteLine($"[DEBUG] OTP generated for {dto.Email}: {otp}");

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Customer added but OTP email could not be sent: " + ex.Message);
                return View(dto);
            }

            return RedirectToAction("VerifyOtpAdmin", new { email = dto.Email });

        }

        //verify otp for Admin
        [HttpGet]
        public IActionResult VerifyOtpAdmin(string email)
        {
            return View(new OtpViewModel { Email = email });
        }

        [HttpPost]
        public async Task<IActionResult> VerifyOtpAdmin(OtpViewModel model)
        {
            if (_otpService.VerifyOtp(model.Email, model.Otp))
            {
                var dtoJson = TempData["AdminregisterData"] as string;
                if (dtoJson != null)
                {
                    var AdminregisterData = JsonSerializer.Deserialize<CustomerDto>(dtoJson);
                    var dto = new CustomerDto
                    {
                        Name = AdminregisterData.Name,
                        Address = AdminregisterData.Address,
                        PhoneNumber = AdminregisterData.PhoneNumber,
                        LicenceNumber = AdminregisterData.LicenceNumber,
                        Email = AdminregisterData.Email,
                        UserName = AdminregisterData.UserName,
                        Password = AdminregisterData.Password
                    };
                    await _service.AddCustomerAsync(dto);
                    _emailService.SendEmail(
                dto.Email,
                $"Welcome to Our Family, {dto.Name}!",
                $"Hello {dto.Name}, welcome aboard! Your login details:\n   Username: {dto.UserName}\n  Password: {dto.Password}\nWarm regards, Car Rental Team");
                    TempData["SuccessMessage"] = $"{dto.Name} has been added successfully!";
                }
                return RedirectToAction("ViewCustomer");
            }
            ModelState.AddModelError("", "Invalid OTP");
            return View(model);
        }


        public async Task<IActionResult> CustomerDetail(Guid id)
        {
            var customer = await _service.GetCustomerIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }


        //Get Customer for Update
        [HttpGet]
        public async Task<IActionResult> Update(Guid id)
        {
            var customer = await _service.GetCustomerIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }


        // Type 01 SuccessMessage ============

        [HttpPost]
        public async Task<IActionResult> Update(CustomerDto dto)
        {
            if (ModelState.IsValid)
            {
                await _service.UpdateCustomerAsync(dto);

                TempData["SuccessMessage"] = $"{dto.Name} updated successfully!";

                return RedirectToAction("ViewCustomer");
            }
            return View(dto);
        }


        //Delete Customer
        public async Task<IActionResult> Delete(Guid id)
        {
            var customer = await _service.GetCustomerIdAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            TempData["SuccessMessage"] = $"{customer.Name} Deleted successfully!";
            await _service.DeleteCustomerAsync(id);
            return RedirectToAction("ViewCustomer");
        }


        //Filter Customer
        [HttpGet]
        public async Task<IActionResult> SearchCustomers([FromQuery] string query)
        {
            var results = await _service.SearchCustomersAsync(query ?? "");
            return Ok(results);
        }
    }
}