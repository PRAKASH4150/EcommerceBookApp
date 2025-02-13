using Microsoft.AspNetCore.Mvc;

namespace EcommerceBookApp.Areas.Customer.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
