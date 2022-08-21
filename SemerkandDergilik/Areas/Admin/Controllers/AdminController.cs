using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.ENTITIES.Models;
using Semerkand_Dergilik.Controllers;
using Semerkand_Dergilik.ViewModels;

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

        // List Users (AdminLayouttan tetiklenir.. asp-action="Users")
        [Route("Users")]
        public IActionResult Users()
        {
            return View(userManager.Users.ToList());
        }

        // List Roles (_AdminLayouttan tetiklenir.. asp-action="Roles")
        [Route("Roles")]
        public IActionResult Roles()
        {
            return View(roleManager.Roles.ToList()); // Roles => IQueryable<TRole> => IQueryable<AppRole>
        }

        [Route("RoleCreate")]
        // Create Roles
        public IActionResult RoleCreate()
        {
            return View();
        }

        [HttpPost]
        [Route("RoleCreate")]
        public IActionResult RoleCreate(RoleViewModel roleViewModel)
        {
            AppRole role = new AppRole();
            role.Name = roleViewModel.Name;
            IdentityResult result = roleManager.CreateAsync(role).Result;

            if (result.Succeeded)

            {
                return RedirectToAction("Roles");
            }
            else
            {
                AddModelError(result);
            }

            return View(roleViewModel);
        }
    }
}
