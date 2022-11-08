using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Semerkand_Dergilik.Areas.Admin.Controllers
{
    [Area("Admin")]   // Admin Areas'a yönelebilmesi için belirtilir..
    [Route("Admin/Admin")] // Home Index'e yönlenebilmesi için Route belirtilir..
    [Authorize(Roles = "Admin")] // case sensitive  
    public class AdminController : Controller
    {
        [Route("")]
        [Route("Index")]
        public IActionResult Index()
        {
            //return View(userManager.Users.ToList());
            return View();
        }
    }
}
