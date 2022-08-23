using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.ENTITIES.Models;
using Semerkand_Dergilik.Controllers;
using Semerkand_Dergilik.ViewModels;
using System.Data;

namespace Semerkand_Dergilik.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin")]
    [Authorize(Roles = "Admin")] // case sensitive  
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

        [HttpPost]
        [Route("RoleDelete")]
        public IActionResult RoleDelete(string id)
        {
            AppRole role = roleManager.FindByIdAsync(id).Result;

            if (role != null)
            {
                IdentityResult result = roleManager.DeleteAsync(role).Result;
            }

            return RedirectToAction("Roles");
        }

        // Güncelleme
        [Route("RoleUpdate")]
        public IActionResult RoleUpdate(string id)
        {
            AppRole role = roleManager.FindByIdAsync(id).Result;

            if (role != null)
            {
                return View(role.Adapt<RoleViewModel>()); // automap
            }

            return RedirectToAction("Roles");
        }

        [HttpPost]
        [Route("RoleUpdate")]
        public IActionResult RoleUpdate(RoleViewModel roleViewModel)
        {
            AppRole role = roleManager.FindByIdAsync(roleViewModel.Id).Result;

            if (role != null)
            {
                role.Name = roleViewModel.Name;
                IdentityResult result = roleManager.UpdateAsync(role).Result;

                if (result.Succeeded)
                {
                    return RedirectToAction("Roles");
                }
                else
                {
                    AddModelError(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "Güncelleme işlemi başarısız oldu.");
            }

            return View(roleViewModel);
        }

        // Rol Atama
        [Route("RoleAssign")]
        public IActionResult RoleAssign(string id)
        {
            TempData["userId"] = id; // RoleAssign post metodunda kullanıldı..

            AppUser user = userManager.FindByIdAsync(id).Result;

            ViewBag.userName = user.UserName;// used in RoleAssign.cshtml

            IQueryable<AppRole> roles = roleManager.Roles; // get all roles from database ([AspNetRoles]) tablosu.

            List<string> userroles = userManager.GetRolesAsync(user).Result as List<string>; // get roles that belongs to user.. .Result => IList döner.. cast to List<>

            // RoleAssignViewModel.cs
            List<RoleAssignViewModel> roleAssignViewModels = new List<RoleAssignViewModel>();

            foreach (var role in roles) //  all roles
            {
                RoleAssignViewModel r = new RoleAssignViewModel();
                r.RoleId = role.Id.ToString();
                r.RoleName = role.Name;

                if (userroles.Contains(role.Name)) // compare user's roles to all roles
                {
                    r.Exist = true; // user own role
                }
                else
                {
                    r.Exist = false; // user not own role
                }
                roleAssignViewModels.Add(r);
            }

            return View(roleAssignViewModels); // RoleAssign.cshtml
        }

        [HttpPost]
        [Route("RoleAssign")]
        public async Task<IActionResult> RoleAssign(List<RoleAssignViewModel> roleAssignViewModels)
        {
            AppUser user = userManager.FindByIdAsync(TempData["userId"].ToString()).Result;

            foreach (var item in roleAssignViewModels)
            {
                if (item.Exist)

                {
                    await userManager.AddToRoleAsync(user, item.RoleName); // assign to
                }
                else
                {
                    await userManager.RemoveFromRoleAsync(user, item.RoleName);
                }
            }

            return RedirectToAction("Users");
        }

    }
}
