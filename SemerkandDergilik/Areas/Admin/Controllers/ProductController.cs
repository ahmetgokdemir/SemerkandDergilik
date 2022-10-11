using Microsoft.AspNetCore.Mvc;

namespace Semerkand_Dergilik.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
