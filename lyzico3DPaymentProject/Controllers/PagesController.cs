using Microsoft.AspNetCore.Mvc;

namespace lyzico3DPaymentProject.Controllers
{
    public class PagesController : Controller
    {
        public IActionResult Home()
        {
            return View();
        }
        public IActionResult Payment()
        {
            return View();
        }
    }
}
