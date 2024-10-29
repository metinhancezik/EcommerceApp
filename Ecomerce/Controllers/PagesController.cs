using lyzico3DPaymentProject.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Iyzico3DPaymentProject.Models;
using ECommerceView.Models;
using Microsoft.AspNetCore.Identity;
using ServiceLayer.Abstract;
using Microsoft.AspNetCore.Mvc.Rendering;
using EntityLayer.Concrete;
using DataAccesLayer.Abstract;

namespace lyzico3DPaymentProject.Controllers
{
    public class PagesController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserDetailService _userDetailService;
        private ICountryService _countryService;
        private IAuthTokensService _authTokensService;
        public PagesController(IHttpContextAccessor httpContextAccessor, IUserDetailService userDetailService, ICountryService countryService, IAuthTokensService authTokensService)
        {
            _httpContextAccessor = httpContextAccessor;
            _userDetailService = userDetailService;
            _countryService = countryService;
            _authTokensService = authTokensService;
        }


        [HttpGet]
        public IActionResult Home()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Basket()
        {
            // FastEndpoint üzerinden sepet verilerini al
            var cart = GetCartFromCookie();
            var productViewModels = cart.Select(item => new ProductViewModel
            {
                Id = item.productId,
                ProductName = item.productName,
                UnitPrice = item.price,
                Stock = item.quantity,
                ImageUrl = $"~/pictures/product{item.productId}.jpg",
                VendorName = "Satıcı"
            }).ToList();

            return View(productViewModels);
        }

        private List<CartItemViewModel> GetCartFromCookie()
        {
            var cart = _httpContextAccessor.HttpContext.Request.Cookies["cart"];
            return string.IsNullOrEmpty(cart) ? new List<CartItemViewModel>() : JsonConvert.DeserializeObject<List<CartItemViewModel>>(cart);
        }


        [HttpGet]
        public IActionResult ProductDetail()
        {
      
            return View();
        }

        [HttpGet]
        public IActionResult Payment()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Account()
        {
            if (User.Identity.IsAuthenticated)
            {
                var token = User.Claims.FirstOrDefault(c => c.Type == "token")?.Value;

                if (string.IsNullOrEmpty(token))
                {
                    // Token yoksa, kullanıcıyı giriş sayfasına yönlendir
                    return RedirectToAction("Login");
                }

                var userIdFromToken = await _authTokensService.GetUserIdFromTokenAsync(token);

                if (userIdFromToken.HasValue)
                {
                    var userDetails = _userDetailService.GetUserByLongId(userIdFromToken.Value);

                    if (userDetails == null)
                    {
                        // Kullanıcı bulunamadıysa, uygun bir hata mesajı döndür
                        ModelState.AddModelError("", "Kullanıcı bilgileri bulunamadı.");
                        return View(new AccountViewModel());
                    }

                    var accountModel = new AccountViewModel
                    {
                        Id = userDetails.Id,
                        Name = userDetails.UserName,
                        Surname = userDetails.UserSurname,
                        GsmNumber = userDetails.UserPhone,
                        Email = userDetails.UserMail,
                        Country = userDetails.Country?.CountryName
                    };

                    // Kullanıcı bilgilerini oturumda sakla
                    _httpContextAccessor.HttpContext.Session.SetString("AccountInfo", JsonConvert.SerializeObject(accountModel));

                    return View(accountModel);
                }
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
