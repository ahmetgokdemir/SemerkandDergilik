using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.ENTITIES.Models;
using Semerkand_Dergilik.Controllers;

namespace Semerkand_Dergilik.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin")]
    public class AdminController : BaseController
    {
        //private UserManager<AppUser> userManager { get; }

        /*public AdminController(UserManager<AppUser> userManager)
        {
            //this.userManager = userManager;
        }*/

        public AdminController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager) : base(userManager, null, roleManager)
        {
        }

        [Route("")]
        [Route("Index")]
        public IActionResult Index()
        {
            //return View(userManager.Users.ToList());
            return View();
        }

        [Route("Users")]
        public IActionResult Users()
        {
            return View(userManager.Users.ToList());
        }
    }
}
