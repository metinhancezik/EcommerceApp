using lyzico3DPaymentProject.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Iyzico3DPaymentProject.Models;
using ECommerceView.Models;
using Microsoft.AspNetCore.Identity;
using ServiceLayer.Abstract;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace lyzico3DPaymentProject.Controllers
{
    public class PagesController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserDetailService _userDetailService;
        private ICountryService _countryService;
        public PagesController(IHttpContextAccessor httpContextAccessor, IUserDetailService userDetailService, ICountryService countryService)
        {
            _httpContextAccessor = httpContextAccessor;
            _userDetailService = userDetailService;
            _countryService = countryService;
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
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return RedirectToAction("Login"); // Kullanıcı yoksa Login sayfasına yönlendir
                }

                // Kullanıcı bilgilerini servis üzerinden alıyoruz
                //var userDetails = _userDetailService.FindByIdentityServerId(userId);

                //if (userDetails == null)
                //{
                //    return View(new AccountViewModel()); // Kullanıcı bulunmazsa boş model döndür
                //}

                // UserDetails'i AccountViewModel'e map ediyoruz
                var accountModel = new AccountViewModel
                {
                    //Id = userDetails.Id,
                    //IdentityServerID = userDetails.IdentityServerId,
                    //Name = userDetails.Name,
                    //Surname = userDetails.Surname,
                    //GsmNumber = userDetails.GsmNumber,
                    //Email = userDetails.Email,
                    //IdentityNumber = userDetails.IdentityNumber,
                    //RegistrationAddress = userDetails.RegistrationAddress,
                    //City = userDetails.City,
                    //Country = userDetails.Country,
                    //ZipCode = userDetails.ZipCode
                };
                _httpContextAccessor.HttpContext.Session.SetString("AccountInfo", JsonConvert.SerializeObject(accountModel));

                return View(accountModel); // Modeli View'e gönderiyoruz
            }
            return RedirectToAction("Home");
           
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
            var countries = _countryService.GetList();

            // Ülke listesini kontrol et
            if (countries == null || !countries.Any())
            {
                ViewBag.Countries = new SelectList(Enumerable.Empty<SelectListItem>()); // Boş bir liste oluştur
                Console.WriteLine("Ülke listesi boş veya null.");
            }
            else
            {
                ViewBag.Countries = new SelectList(countries, "Id", "CountryName"); // Doğru alan adını kullanın
                foreach (var country in countries)
                {
                    Console.WriteLine($"Ülke ID: {country.Id}, Ülke Adı: {country.CountryName}");
                }
            }

            return View();
        }

    }
}
