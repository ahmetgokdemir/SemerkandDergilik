using Microsoft.AspNetCore.Mvc;

namespace Semerkand_Dergilik.Controllers
{
    public class MemberController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
