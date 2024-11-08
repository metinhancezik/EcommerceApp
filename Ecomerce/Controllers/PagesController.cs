using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using ServiceLayer.Abstract;
using Microsoft.AspNetCore.Mvc.Rendering;
using EntityLayer.Concrete;
using DataAccesLayer.Abstract;
using ECommerceView.Models.Cart;
using ECommerceView.Models.Account;
using ECommerceView.Models.Product;
using ECommerceView.Models.Orders;


namespace lyzico3DPaymentProject.Controllers
{
    public class PagesController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUserDetailService _userDetailService;
        private ICountryService _countryService;
        private IAuthTokensService _authTokensService;
        private IProductsService _productsService;
        private ICart _cartService;
        private ICartItems _cartItemsService;
        private ICity _cityService;
        public PagesController(IHttpContextAccessor httpContextAccessor, IUserDetailService userDetailService, 
            ICountryService countryService, IAuthTokensService authTokensService, IProductsService productsService, 
            ICart cartService, ICartItems cartItemsService, ICity cityService)
        {
            _httpContextAccessor = httpContextAccessor;
            _userDetailService = userDetailService;
            _countryService = countryService;
            _authTokensService = authTokensService;
            _productsService = productsService;
            _cartService = cartService;
            _cartItemsService = cartItemsService;
            _cityService= cityService;
        }


        [HttpGet]
        public async Task<IActionResult> Home()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userToken = User.Claims.FirstOrDefault(c => c.Type == "token")?.Value;
                if (!string.IsNullOrEmpty(userToken))
                {
                    var userId = await _authTokensService.GetUserIdFromTokenAsync(userToken);
                    if (userId.HasValue)
                    {
                        var cartInfo = await _cartService.GetByUserId(userId.Value);
                        if (cartInfo != null)
                        {
                            
                            var cartItems = await _cartItemsService.GetCartItemsByCartId(cartInfo.Id);

                            var cartItemDetails = new List<CartItemViewModel>();

                            foreach (var item in cartItems)
                            {
                                var product = _productsService.GetProductByLongId(item.ProductId);
                                if (product != null)
                                {
                                    cartItemDetails.Add(new CartItemViewModel
                                    {
                                        productId =item.Id,
                                        productName = product.ProductName,
                                        quantity = item.Quantity,
                                        price = item.UnitPrice,
                                        imageUrl = $"~/pictures/product{item.ProductId}.jpg"
                                    });
                                }
                            }

                            ViewBag.CartItems = cartItemDetails;
                            ViewBag.CartItemsCount = cartItems.Count;
                            ViewBag.CartTotalPrice = cartItems.Sum(x => x.TotalPrice);
                        }
                        else
                        {
                            ViewBag.CartItems = new List<CartItems>();
                            ViewBag.CartTotalPrice = 0;
                            ViewBag.CartItemsCount = 0;
                        }
                    }
                }
            }

            var products = _productsService.GetList();

            return View(products);
        }

        [HttpGet]
        public async Task<IActionResult> Basket()
        {
            List<ProductViewModel> productViewModels;

            if (User.Identity.IsAuthenticated)
            {
                var userToken = User.Claims.FirstOrDefault(c => c.Type == "token")?.Value;
                if (!string.IsNullOrEmpty(userToken))
                {
                    var userId = await _authTokensService.GetUserIdFromTokenAsync(userToken);
                    if (userId.HasValue)
                    {
                        var cartInfo = await _cartService.GetByUserId(userId.Value);
                        if (cartInfo != null)
                        {
                            var cartItems = await _cartItemsService.GetCartItemsByCartId(cartInfo.Id);
                            productViewModels = new List<ProductViewModel>();

                            foreach (var item in cartItems)
                            {
                                var product = _productsService.GetProductByLongId(item.ProductId);
                                if (product != null)
                                {
                                    productViewModels.Add(new ProductViewModel
                                    {
                                        Id = item.ProductId,
                                        ProductName = product.ProductName,
                                        UnitPrice = item.UnitPrice,
                                        Stock = item.Quantity,
                                        ImageUrl = $"~/pictures/product{item.ProductId}.jpg",
                                        VendorName = "Satıcı"
                                    });
                                }
                            }

                          
                            ViewBag.CartItems = cartItems;
                            ViewBag.CartItemsCount = cartItems.Count;
                            ViewBag.CartTotalPrice = cartItems.Sum(x => x.TotalPrice);
                        }
                        else
                        {
                            productViewModels = new List<ProductViewModel>();
                            ViewBag.CartItems = new List<CartItems>();
                            ViewBag.CartTotalPrice = 0;
                            ViewBag.CartItemsCount = 0;
                        }
                    }
                    else
                    {
                        productViewModels = new List<ProductViewModel>();
                    }
                }
                else
                {
                    productViewModels = new List<ProductViewModel>();
                }
            }
            else
            {
                
                var cart = GetCartFromCookie();
                productViewModels = cart.Select(item => new ProductViewModel
                {
                    Id = item.productId,
                    ProductName = item.productName,
                    UnitPrice = item.price,
                    Stock = item.quantity,
                    ImageUrl = $"~/pictures/product{item.productId}.jpg",
                    VendorName = "Satıcı"
                }).ToList();

              
                ViewBag.CartItems = cart;
                ViewBag.CartItemsCount = cart.Count;
                ViewBag.CartTotalPrice = cart.Sum(x => x.price * x.quantity);
            }

            return View(productViewModels);
        }

  
        private List<CartItemViewModel> GetCartFromCookie()
        {
            try
            {
                var cartCookie = _httpContextAccessor.HttpContext.Request.Cookies["cart"];
                if (string.IsNullOrEmpty(cartCookie))
                    return new List<CartItemViewModel>();

                var cart = JsonConvert.DeserializeObject<List<CartItemViewModel>>(cartCookie);
                return cart ?? new List<CartItemViewModel>();
            }
            catch (Exception ex)
            {
                
                Console.WriteLine($"Cookie parsing error: {ex.Message}");
                return new List<CartItemViewModel>();
            }
        }

        [HttpGet]
        public IActionResult ProductDetail()
        {
      
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Payment()
        {
            decimal totalAmount = 0;
            int itemCount = 0;

            if (User.Identity.IsAuthenticated)
            {
                var userToken = User.Claims.FirstOrDefault(c => c.Type == "token")?.Value;
                if (!string.IsNullOrEmpty(userToken))
                {
                    var userId = await _authTokensService.GetUserIdFromTokenAsync(userToken);
                    if (userId.HasValue)
                    {
                        var cartInfo = await _cartService.GetByUserId(userId.Value);
                        if (cartInfo != null)
                        {
                            var cartItems = await _cartItemsService.GetCartItemsByCartId(cartInfo.Id);
                            totalAmount = cartItems.Sum(x => x.TotalPrice);
                            itemCount = cartItems.Count;
                        }
                    }
                }
            }
            else
            {
                var cart = GetCartFromCookie();
                totalAmount = cart.Sum(x => x.price * x.quantity);
                itemCount = cart.Count;
            }

            ViewBag.TotalAmount = totalAmount;
            ViewBag.ItemCount = itemCount;

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> PersonalInfo()
        {
            var model = new PersonalInfoViewModel();


            if (User.Identity.IsAuthenticated)
            {
                var userToken = User.Claims.FirstOrDefault(c => c.Type == "token")?.Value;
                if (!string.IsNullOrEmpty(userToken))
                {
                    var userId = await _authTokensService.GetUserIdFromTokenAsync(userToken);
                    if (userId.HasValue)
                    {

                      
                        var userInfo = _userDetailService.GetUserByLongId(userId.Value);

                        var cities = _cityService.GetCitiesByCountryId(userInfo.CountryId);
                        ViewBag.Country= userInfo.CountryId;
                        if (userInfo != null)
                        {
                            model.Name = userInfo.UserName;
                            model.Surname = userInfo.UserSurname;
                            model.Phone = userInfo.UserPhone;

                       
                            model.Cities = _cityService.GetCitiesByCountryId(userInfo.CountryId)
                            .Select(city => new SelectListItem
                            {
                                Value = city.Id.ToString(),
                                Text = city.CityName
                            }).ToList();
                        }
                    }
                }
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Account()
        {
            if (User.Identity.IsAuthenticated)
            {
                var token = User.Claims.FirstOrDefault(c => c.Type == "token")?.Value;

                if (string.IsNullOrEmpty(token))
                {
                  
                    return RedirectToAction("Login");
                }

                var userIdFromToken = await _authTokensService.GetUserIdFromTokenAsync(token);

                if (userIdFromToken.HasValue)
                {
                    var userDetails = _userDetailService.GetUserByLongId(userIdFromToken.Value);

                    if (userDetails == null)
                    {
                       
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
              
                _httpContextAccessor.HttpContext.Session.SetString("AccountInfo", JsonConvert.SerializeObject(model));
                return RedirectToAction("Account");
            }
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

      
            if (countries == null || !countries.Any())
            {
                ViewBag.Countries = new SelectList(Enumerable.Empty<SelectListItem>()); 
                Console.WriteLine("Ülke listesi boş veya null.");
            }
            else
            {
                ViewBag.Countries = new SelectList(countries, "Id", "CountryName"); 
                foreach (var country in countries)
                {
                    Console.WriteLine($"Ülke ID: {country.Id}, Ülke Adı: {country.CountryName}");
                }
            }

            return View();
        }

    }
}
