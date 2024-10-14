using ECommerceView.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using DataAccesLayer.Concrete;
using EntityLayer.Concrete;
using System;
using System.Threading.Tasks;
using ServiceLayer.Abstract;
using ServiceLayer.Concrete;

namespace ECommerceView.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly Context _context;
        private readonly IUserDetailService _userDetailService;
        public AccountController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            Context context,
            IUserDetailService userDetailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _userDetailService = userDetailService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await SaveAdditionalUserInfo(user.Id, model);
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Home", "Pages");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                
            }

            return RedirectToAction("Register", "Pages", model);
        }

        private async Task SaveAdditionalUserInfo(string userId, RegisterViewModel model)
        {
            var userDetails = new UserDetails
            {
                IdentityServerId = userId,
                Name = model.Name,
                Surname = model.Surname,
                Email = model.Email,
                GsmNumber = model.GsmNumber,
                IdentityNumber = model.IdentityNumber,
                RegistrationAddress = model.RegistrationAddress,
                City = model.City,
                Country = model.Country,
                ZipCode = model.ZipCode,
                CreatedAt = DateTime.Now
            };
            _userDetailService.TAdd(userDetails);
        
            await _context.SaveChangesAsync();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Home", "Pages");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Geçersiz giriş denemesi.");
                }
            }
                else
                {
                    ModelState.AddModelError(string.Empty, "Geçersiz giriş denemesi.");
                }
            

            return RedirectToAction("Login", "Pages", model);
        }

        [HttpGet]
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Home", "Pages");
        }
    }
}