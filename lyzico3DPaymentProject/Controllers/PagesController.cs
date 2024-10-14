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

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

    }
}
