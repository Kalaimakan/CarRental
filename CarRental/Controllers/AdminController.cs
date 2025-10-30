using CarRental.Interfaces;
using CarRental.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace CarRental.Controllers
{
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet]
        public IActionResult AdminLogin()
        {
            return View(new LoginViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> AdminLogin(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var admin = await _adminService.LoginAsync(model.Identifier, model.Password);

            if (admin != null)
            {
                TempData["SuccessMessage"] = "Login successful! 🎉";
                return RedirectToAction("AdminDashboard");
            }

            ModelState.AddModelError("", "Invalid username/email or password");
            return View(model);
        }

        public IActionResult AdminDashboard()
        {
            ViewData["Title"] = "Dashboard";
            return RedirectToAction("OverView");
        }

        public IActionResult OverView()
        {
            return View();
        }

        public IActionResult Report()
        {
            return View();
        }

        public IActionResult Settings()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> AdminLogout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("AdminLogin");
        }
    }
}
