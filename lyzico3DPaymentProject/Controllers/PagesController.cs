using lyzico3DPaymentProject.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Iyzico3DPaymentProject.Models;
using ECommerceView.Models;
using Microsoft.AspNetCore.Identity;

namespace lyzico3DPaymentProject.Controllers
{
    public class PagesController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PagesController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Home()
        {
            return View();
        }
        public IActionResult Payment()
        {
            return View();
        }

        public IActionResult Account()
        {
            var accountInfo = HttpContext.Session.GetString("AccountInfo");
            if (!string.IsNullOrEmpty(accountInfo))
            {
                return View(JsonConvert.DeserializeObject<AccountViewModel>(accountInfo));
            }
            return View(new AccountViewModel());
        }

        [HttpPost]
        public IActionResult SaveAccount(AccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Model geçerliyse, session'a kaydet
                _httpContextAccessor.HttpContext.Session.SetString("AccountInfo", JsonConvert.SerializeObject(model));
                return RedirectToAction("Account");
            }

            // Model geçerli değilse, hataları göster ve formu tekrar göster
            return View("Account", model);
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        //[HttpPost]
        //public async Task<IActionResult> Login(LoginViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

        //        if (result.Succeeded)
        //        {
        //            return RedirectToAction("Index", "Home");
        //        }

        //        ModelState.AddModelError(string.Empty, "Geçersiz giriş denemesi.");
        //    }

        //    return View(model);
        //}

        [HttpGet]

        public IActionResult Register()
        {
            return View();
        }
        //[HttpPost]
        //public async Task<IActionResult> Register(RegisterViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var user = new ApplicationUser
        //        {
        //            UserName = model.Email,
        //            Email = model.Email,
        //            // Diğer özel alanları doldurun
        //        };

        //        var result = await _userManager.CreateAsync(user, model.Password);

        //        if (result.Succeeded)
        //        {
        //            // Kullanıcı başarıyla oluşturuldu
        //            // DataAccessLayer kullanarak ek bilgileri kaydedin
        //            await _dataAccessLayer.SaveUserDetails(user.Id, model);

        //            // Kullanıcıyı otomatik olarak giriş yaptırın
        //            await _signInManager.SignInAsync(user, isPersistent: false);

        //            return RedirectToAction("Index", "Home");
        //        }

        //        foreach (var error in result.Errors)
        //        {
        //            ModelState.AddModelError(string.Empty, error.Description);
        //        }
        //    }

        //    return View(model);
        //}



    }
}
