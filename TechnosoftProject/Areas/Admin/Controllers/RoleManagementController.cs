using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project.ENTITIES.Identity_Models;
using Project.ENTITIES.Models;
using Technosoft_Project.Controllers;
using Technosoft_Project.ViewModels;
using Technosoft_Project.VMClasses;
using System.Data;

namespace Technosoft_Project.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/RoleManagement")]    
    [Authorize(Roles = "Admin")] // case sensitive  
    public class RoleManagementController : BaseController
    {
        //private UserManager<AppUser> userManager { get; }

        /*public AdminController(UserManager<AppUser> userManager)
        {
            //this.userManager = userManager;
        }*/

        public RoleManagementController(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager) : base(userManager, null, roleManager)
        {
        }

        [Route("")]
        [Route("Index")] // [Route("Index")] --> Admin Index ile çakışır.. -->     [Route("Admin/RoleManagement")] eklenince düzeldi..  
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
            // return View(roleManager.Roles.ToList()); // Roles => IQueryable<TRole> => IQueryable<AppRole> => convert to list

            List<AppRole> roleName = roleManager.Roles.ToList();

            return View(roleName.Adapt<List<RoleViewModel>>()); // automap

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

        // DeleteRoleAjax
        [Route("DeleteRoleAjax")]
        public async Task<PartialViewResult> DeleteRoleAjax(string id) // id'i int değil!!
        {
            AppRole role = roleManager.FindByIdAsync(id).Result;

            role.Adapt<List<RoleViewModel>>();

            ViewBag.RoleNameDelete = role.Name;

            return PartialView("_DeleteRolePartial", role.Adapt<RoleViewModel>());
 
        }

        [HttpPost]
        [Route("RoleDelete")]
        public IActionResult RoleDelete(string id, RoleViewModel role_control) // string id <---> asp-route-id="@item.Id", id isminin verilmesinin nedeni Program.cs kısmındaki'MapControllerRoute' ....er=Home}/{action=Index}/{id?}");
        {
            AppRole role = roleManager.FindByIdAsync(id).Result;
            // AppRole role_yedek = role_control.Adapt<AppRole>(); --> bu yöntem çalışmadı!!

            /*
              <input type="hidden" asp-for="@item.Id" /> html'de işe yaramadı dolayısıyla aşağıdaki kod da geçersiz...
              
            public IActionResult RoleDelete(RoleViewModel roleViewModel)
            {              
                AppRole role = roleManager.FindByIdAsync(roleViewModel.Id).Result;
            
            */

            if (/*role_yedek != null &&*/ role != null)
            {
                //IdentityResult result_2 = roleManager.DeleteAsync(role).Result;
                IdentityResult result = roleManager.DeleteAsync(role).Result;
                
                if (result.Succeeded)
                {
                    return RedirectToAction("Roles");
                }
                else
                {
                    AddModelError(result);
                }
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
            AppRole role = roleManager.FindByIdAsync(roleViewModel.Id).Result; // roleViewModel içerindeki Id'i Update işleminde fayda sağlıyor..

            if (role != null) // bu kontrole gerek yok gibi... gerekli olabilir user bu sayfada iken veritabanından veri silinirse o zaman hata oluşur..
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

        // Rol Atama (AspNetUserRoles ve AspNetRoles tabloları)
        [Route("RoleAssign")]
        public IActionResult RoleAssign(string id)
        {
            TempData["userId"] = id; // RoleAssign post metodunda kullanıldı..

            AppUser user = userManager.FindByIdAsync(id).Result; // currentuser kullanmayacağız.. AppUser user cookieden gelen değer değil (currentuser, login olan kullanıcıyı temsil eder) yani aşağıdaki kod değil..

            /*             
                AppUser user = userManager.FindByNameAsync(HttpContext.User.Identity.Name).Result;
                //AppUser user = CurrentUser;              
             */

            ViewBag.userName = user.UserName;// used in RoleAssign.cshtml

            IQueryable<AppRole> roles = roleManager.Roles; // get all roles from database ([AspNetRoles]) tablosu.

            List<string> userroles = userManager.GetRolesAsync(user).Result as List<string>; // get roles that belongs to user from AspNetUserRoles tablosundan.. .Result => IList döner.. cast to List<>

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
                    await userManager.AddToRoleAsync(user, item.RoleName); // assign to AspNetUserRoles tablosuna
                    // userManager.AddToRoleAsync(user, item.RoleName).Result; RESULT HATA VERİYOR.. async Task<> & await KULLANILMALI
                }
                else
                {
                    await userManager.RemoveFromRoleAsync(user, item.RoleName); // remove from AspNetUserRoles tablosundan
                    // userManager.RemoveFromRoleAsync(user, item.RoleName).Result; RESULT HATA VERİYOR..
                }
            }

            return RedirectToAction("Users");
        }

        // show default claims
        [Route("Claims")]
        public IActionResult Claims()
        {
            return View(User.Claims.ToList());
        }


        // ResetUserPassword by Admin
        [Route("ResetUserPassword")]
        public async Task<IActionResult> ResetUserPassword(string id)
        {
            AppUser user = await userManager.FindByIdAsync(id);

            // PasswordResetByAdminViewModel passwordResetByAdminViewModel = new PasswordResetByAdminViewModel();
            // passwordResetByAdminViewModel.Id = user.Id.ToString();
            // passwordResetByAdminViewModel.UserName = user.UserName;
            // passwordResetByAdminViewModel.Email = user.Email;

            PasswordResetByAdminViewModel passwordResetByAdminViewModel = user.Adapt<PasswordResetByAdminViewModel>(); // automap


            return View(passwordResetByAdminViewModel);
        }


        [HttpPost]
        [Route("ResetUserPassword")]
        public async Task<IActionResult> ResetUserPassword(PasswordResetByAdminViewModel passwordResetByAdminViewModel)
        {
            AppUser user = await userManager.FindByIdAsync(passwordResetByAdminViewModel.Id);

            string token = await userManager.GeneratePasswordResetTokenAsync(user);

            IdentityResult result = await userManager.ResetPasswordAsync(user, token, passwordResetByAdminViewModel.NewPassword);

            if (result.Succeeded)
            {
                await userManager.UpdateSecurityStampAsync(user);
                return RedirectToAction("Users");
            }
            else
            {
                AddModelError(result);
            }

            return View(passwordResetByAdminViewModel);

            //securitystamp degerini  update etmezsem kullanıcı eski şifresiyle sitemizde dolaşmaya devam eder ne zaman çıkış yaparsa ozaman tekrar yeni şifreyle girmek zorunda
            //eger update edersen kullanıcı  otomatik olarak  sitemize girdiği zaman login ekranına yönlendirilecek.

            //Identity Mimarisi cookie tarafındaki securitystamp ile veritabanındaki security stamp değerini her 30 dakikada bir kontrol eder. Kullanıcı eski şifreyle en fazla server da session açıldıktan sonra 30 dakkika gezebilir. Bunu isterseniz 1 dakkikaya indirebilirsiniz. ama tavsiye edilmez. her bir dakika da  her kullanıcı için veritabanı kontrolü  yük getirir.


        }


        //Confirm_Member_Ajax
        [HttpPost]
        [Route("Confirm_Member_Ajax")]
        public async Task<string> Confirm_Member_Ajax(string member_ID, string name)
        {
            string kontrol = "asd"; 
            return member_ID;
        }




        }
}
