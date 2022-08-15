using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.ENTITIES.Models;

namespace Semerkand_Dergilik.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin")]
    public class AdminController : Controller
    {
        private UserManager<AppUser> userManager { get; }

        public AdminController(UserManager<AppUser> userManager)
        {
            this.userManager = userManager;
        }

        [Route("")]
        [Route("Index")]
        public IActionResult Index()
        {
            return View(userManager.Users.ToList());
        }
    }
}
